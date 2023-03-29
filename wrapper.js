// Adds Accept BLue iFrame script element to the head
// Is wrapped in a function thats automatically called to limit the varaible namespace
(() => {
	let script = document.createElement("script");
	script.src = "https://tokenization.develop.accept.blue/tokenization/v0.2";
	script.id = "_iFrame";
	document.head.appendChild(script);
})();

const _dataAttributeType = "value"; //used to easily set what data attribute you'd like. Using convention _ to indicate its a variable not to be used
//Used to Get an element based on id and Set a method to it ex: setAttribute, submit, and textContent - setAttribute is default
const _getAndSet = (id) => {
	Object.entries(id).forEach((e) => {
		const el = document.getElementById(e[0]);
		if (e[1]["submit"] || e[1]["text"]) {
			e[1]["submit"] ? el.submit() : (el.textContent = e[1]["value"]);
		}
		el.setAttribute(_dataAttributeType, e[1]["value"]); // default
	});
};
// Class that wraps Accept.blue iFrame
class HostedIFrame {
	cardForm;
	constructor(tokenSourceKey, iframeMount, btnMount) {
		this.tokenSourceKey = tokenSourceKey;
		this.iframeMount = `#${iframeMount}`;
		this.btnMount = `${btnMount}`;
	}

	init() {
		this._onLoad(() => {
			this.cardForm = new window.HostedTokenization(this.tokenSourceKey)
				.create("card-form")
				.mount(this.iframeMount);
		});

		return this;
	}
	//Submits and mounts the result of the card form. Returns the nonce token, expirymonth, expiryyear, and cardtype and last4
	//Takes an object as the argument
	submit(submitMounts) {
		this._clicked(this.btnMount, () => {
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
		this._onLoad(() => this.cardForm.setStyles(styles));
	}

	_errorMount(errorMount, _mainError, textContent = false) {
		_getAndSet({ [errorMount]: { value: _mainError, text: textContent } });
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
		_getAndSet({
			[tokenMount]: { value: result.nonce },
			[expiryMonthMount]: { value: result.expiryMonth },
			[expiryYearMount]: { value: result.expiryYear },
			[cardTypeMount]: { value: result.cardType },
			[last4Mount]: { value: result.last4 },
			[formMount]: { submit: true },
		});
	}
	//takes an arrow function as a parameter
	_onLoad(injectedCode) {
		window.addEventListener("load", injectedCode);
	}
	//event listener to see if element based on Id was clicked or not
	_clicked(id, injectedCode) {
		document.getElementById(id).addEventListener("click", injectedCode);
	}

	_VerifyAndSave() {
		//code
	}

	Charge() {
		//code
	}
	_ajaxCall() {
		//code
	}
}
