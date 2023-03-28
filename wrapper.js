// Adds Accept BLue iFrame script element to the head
// Is wrapped in a function thats automatically called to limit the varaible namespace
(() => {
	let script = document.createElement("script");
	script.src = "https://tokenization.develop.accept.blue/tokenization/v0.2";
	script.id = "iFrame";
	document.head.appendChild(script);
})();

// Class that wraps Accept.blue iFrame
class HostedIFrame {
	cardForm; //private field
	constructor(tokenSourceKey, iframeMount, btnMount) {
		this.tokenSourceKey = tokenSourceKey;
		this.iframeMount = `#${iframeMount}`;
		this.btnMount = `#${btnMount}`;
	}

	init() {
		//First checks to see if the script tag has been added before running code
		const mutationObserver = new MutationObserver((entries) => {
			entries.forEach((e) => {
				if ((e.addedNodes[0].id = "iFrame")) {
					const tokenizationSourceKey = this.tokenSourceKey;
					const hostedTokenization = new window.HostedTokenization(
						tokenizationSourceKey
					);
					//Creates and mounts the credit card form
					this.cardForm = hostedTokenization
						.create("card-form")
						.mount(this.iframeMount);
				}
			});
		});
		const parent = document.querySelector("head");
		mutationObserver.observe(parent, { childList: true });
		return this;
	}

	//Submits and mounts the result of the card form. Returns the nonce token, expirymonth, expiryyear, and cardtype and last4
	//Takes an object as the argument
	submit(submitMounts) {
		$(this.btnMount).click(() => {
			const resultMount = this._resultMount; //set outside the promise to access HostedIFrame
			const errorResult = this._errorMount; //set outside the promise to access HostedIFrame
			this.cardForm
				.getNonceToken()
				.then((result) => {
					resultMount(
						result,
						submitMounts.form,
						submitMounts.token,
						submitMounts.expiryMonth,
						submitMounts.expiryYear,
						submitMounts.cardType,
						submitMounts.last4
					);
				})
				.catch((mainError) => {
					let error = ("" + mainError).replace("Error: ", "");
					errorResult(submitMounts.mountError, error, submitMounts.textContent);
					throw new Error(error);
				});
		});
	}

	//used to preset styles based on an object that will be provided with multiple items within that will change the style
	styles(styles) {
		const mutationObserver = new MutationObserver((entries) => {
			entries.forEach((e) => {
				if ((e.addedNodes[0].id = "iFrame")) {
					this.cardForm.setStyles(styles);
				}
			});
		});
		const parent = document.querySelector("head");
		mutationObserver.observe(parent, { childList: true });
	}

	_errorMount(errorMount, _mainError, textContent = false) {
		const eMount = $(`#${errorMount}`);
		if (textContent) {
			eMount.text(_mainError);
		}
		eMount.attr("value", _mainError);
	}
	_resultMount(
		result,
		formMount,
		tokenMount,
		expiryMonthMount,
		expiryYearMount,
		cardTypeMount,
		last4Mount
	) {
		$(`#${tokenMount}`).attr("value", result.nonce);
		$(`#${expiryMonthMount}`).attr("value", result.expiryMonth);
		$(`#${expiryYearMount}`).attr("value", result.expiryYear);
		$(`#${cardTypeMount}`).attr("value", result.cardType);
		$(`#${last4Mount}`).attr("value", result.last4);
		$(`#${formMount}`).submit();
	}
}
