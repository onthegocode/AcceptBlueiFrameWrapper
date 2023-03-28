// Adds Accept BLue iFrame script element to the head
// Is wrapped in a function thats automatically called to limit the varaible namespace
(() => {
	let script = document.createElement("script");
	script.src = "https://tokenization.develop.accept.blue/tokenization/v0.2";
	script.id = "_iFrame";
	document.head.appendChild(script);
})();

const _dataAttributeType = "value"; //used to easily set what data attribute you'd like. Using convention _ to indicate its a variable not to be used

// Class that wraps Accept.blue iFrame
class HostedIFrame {
	cardForm;
	#scriptMount = "_iFrame"; //private field
	#observeParent = document.querySelector("head");
	constructor(tokenSourceKey, iframeMount, btnMount) {
		this.tokenSourceKey = tokenSourceKey;
		this.iframeMount = `#${iframeMount}`;
		this.btnMount = `#${btnMount}`;
	}

	init() {
		this._observe(() => {
			const tokenizationSourceKey = this.tokenSourceKey;
			const hostedTokenization = new window.HostedTokenization(
				tokenizationSourceKey
			);
			this.cardForm = hostedTokenization
				.create("card-form")
				.mount(this.iframeMount);
		});
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
		this._observe(() => {
			this.cardForm.setStyles(styles);
		});
	}

	_errorMount(errorMount, _mainError, textContent = false) {
		const eMount = $(`#${errorMount}`);

		eMount.attr(_dataAttributeType, _mainError);

		if (textContent) {
			eMount.text(_mainError);
		}
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
		$(`#${tokenMount}`).attr(_dataAttributeType, result.nonce);
		$(`#${expiryMonthMount}`).attr(_dataAttributeType, result.expiryMonth);
		$(`#${expiryYearMount}`).attr(_dataAttributeType, result.expiryYear);
		$(`#${cardTypeMount}`).attr(_dataAttributeType, result.cardType);
		$(`#${last4Mount}`).attr(_dataAttributeType, result.last4);
		$(`#${formMount}`).submit();
	}

	//takes an arrow function as a parameter
	_observe(injectedCode) {
		{
			//First checks to see if the script tag has been added before running code
			const mutationObserver = new MutationObserver((entries) => {
				entries.forEach((e) => {
					if ((e.addedNodes[0].id = this.#scriptMount)) {
						injectedCode(); //any code you write in function will be here
					}
				});
			});
			mutationObserver.observe(this.#observeParent, { childList: true });
		}
	}
}
