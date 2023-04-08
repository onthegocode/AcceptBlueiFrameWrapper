// Adds Accept Blue iFrame script element to the head
// Is wrapped in a function that automatically called to limit the variable namespace
(() => {
	let script = document.createElement("script");
	script.src = "https://tokenization.develop.accept.blue/tokenization/v0.2"; //Change to Config 
	script.id = "_iFrame";
	document.head.appendChild(script);
})();

// Class that wraps Accept.blue iFrame
class HostedIFrame {
	cardForm; //used to store the form

	constructor(tokenSourceKey, iframeMount) {
		this.tokenSourceKey = tokenSourceKey;
		this.iframeMount = `#${iframeMount}`;

		//Auto Initiate
		this._onLoad(() => {
			this.cardForm = new window.HostedTokenization(this.tokenSourceKey)
				.create("card-form")
				.mount(this.iframeMount);
		});
	}

	//Submits and mounts the result of the card form. Returns a Json response that contains the nonce token, expirymonth, expiryyear, and cardtype and last4
	//Takes an object as the argument

	submit(data = {}) {
		var self = this;
		return new Promise(function (resolve, reject) {
			self.cardForm
				.getNonceToken()
				.then((result) => {
					//calls the api to verify nonce token and returns token
					const newObj = {
						Name: data.Name,
						Avs_Address: data.Avs_Address,
						Avs_Zip: data.Avs_Zip,
						Software: data.Software,
						Source: "nonce-" + result.nonce,
						Expiry_Month: result.expiryMonth,
						Expiry_Year: result.expiryYear,
					}

					$.ajax({
						url: "/api/transactions/sourceverification",
						contentType: "application/json",
						data: JSON.stringify(newObj),
						type: "POST",
						success: function (data) {
							resolve(data); //returns the object data
						},
						error: function (error) {
							reject(error); //returns the error
						},
					});
				})
				.catch((mainError) => {
					reject(mainError); //returns the error
				});
		});
	}
	//used to preset styles based on an object that will be provided with multiple items within that will change the style
	styles(styles) {
		this._onLoad(() => this.cardForm.setStyles(styles));
		return this;
	}

	//Checks to see if the page was loaded before running a function
	//takes a function as a parameter
	_onLoad(injectedCode) {
		window.addEventListener("load", injectedCode);
	}
}
