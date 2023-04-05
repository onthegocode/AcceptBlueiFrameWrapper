// Adds Accept Blue iFrame script element to the head
// Is wrapped in a function that automatically called to limit the variable namespace
(() => {
	let script = document.createElement("script");
	script.src = "https://tokenization.develop.accept.blue/tokenization/v0.2";
	script.id = "_iFrame";
	document.head.appendChild(script);
})();

const _dataAttributeType = "value"; //Used to easily set the data attribute. Using convention _ to indicate its a variable not to be used

//Used to Get an element based on id and Set a method to it ex: setAttribute, submit, and textContent - setAttribute is always set
const _getAndSet = (id) => {
	Object.entries(id).forEach((e) => {
		const el = document.getElementById(e[0]);
		if (e[1]["submit"] || e[1]["text"]) {
			e[1]["submit"] ? el.submit() : (el.textContent = e[1]["value"]);
		}
		el.setAttribute(_dataAttributeType, e[1]["value"]); // default
	});
};

//Used to call the SourceCharge Method inside the TransactionController. This function would likely have to be wrapped in a click event to know when to go off. Takes a token as an argument
function charge(object) {
	return new Promise(function (resolve, reject) {
		$(document).ready(function () {
			$.ajax({
				url: "/api/transactions/sourcecharge",
				contentType: "application/json",
				data: JSON.stringify(object),
				type: "POST",
				success: function (data) {
					resolve(data);
				},
				error: function (error) {
					reject(error);
				},
			});
		});
	});
}

//Used to call the refund Method inside the TransactionController to process a refund. This function takes an object as an argument for the request
function refund(data) {
	$(document).ready(function () {
		$("#btnOkRefund").click(function () {
			$.ajax({
				url: "/api/transactions/refund",
				beforeSend: function () {
					$("#btnCancelRefund").attr("disabled", true);
					$("#btnOkRefund").attr("disabled", true);
				},
				contentType: "application/json",
				data: JSON.stringify(data),
				type: "POST",
				success: function (data) {
					if (!data.error) {
						$("#txtRefundSuccess").text("Refund processed");
						$("#txtRefundFailure").text("");
					} else {
						$("#txtRefundSuccess").text("");
						$("#txtRefundFailure").text("Payment Id not found");
					}
					$("#btnCancelRefund").removeAttr("disabled");
					$("#btnOkRefund").removeAttr("disabled");
					$("#modalRefundAmount").modal("hide");
					$("#modalRefundResult").modal("show");
				},
				error: function (error) {
					$("#btnCancelRefund").removeAttr("disabled");
					$("#btnOkRefund").removeAttr("disabled");
					$("#txtRefundSuccess").text("");
					$("#txtRefundFailure").text(error.responseJSON.message);
					$("#modalRefundResult").modal("show");
				},
			});
		});
	});
}

// Class that wraps Accept.blue iFrame
class HostedIFrame {
	cardForm;
	constructor(tokenSourceKey, iframeMount, btnMount) {
		this.tokenSourceKey = tokenSourceKey;
		this.iframeMount = `#${iframeMount}`;
		this.btnMount = `${btnMount}`;
		this._onLoad(() => {
			this.cardForm = new window.HostedTokenization(this.tokenSourceKey)
				.create("card-form")
				.mount(this.iframeMount);
		});
	}

	/*init() {
		

		return this;
	}*/
	//Submits and mounts the result of the card form. Returns the nonce token, expirymonth, expiryyear, and cardtype and last4
	//Takes an object as the argument
	submit(submitMounts) {
		var main = this;
		return new Promise(function (resolve, reject) {
			main._clicked(main.btnMount, () => {
				/*// const resultMount = this._resultMount; //set outside the promise to access HostedIFrame
				const errorResult = main._errorMount; //set outside the promise to access HostedIFrame
				const sourceVerification = main._sourceVerification; //set outside the promise to access HostedIFrame*/
				main.cardForm
					.getNonceToken()
					.then((result) => {
						//calls the api to verify nonce token and returns token
						const newObj = {
							Name: submitMounts.Name ? submitMounts.Name : "",
							Avs_Address: submitMounts.Avs_Address ? submitMounts.Avs_Address : "",
							Avs_Zip: submitMounts.Avs_Zip ? submitMounts.Avs_Zip : "",
							Software: submitMounts.Software ? submitMounts.Software : "Hi :D",
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

						/*_getAndSet({ [submitMounts.form]: { submit: true } });*/ //submits the form
					})
					.catch((mainError) => {
						reject(mainError); //returns the error 
					});
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

	//event listener to see if the element based on Id was clicked or not
	_clicked(id, injectedCode) {
		document.getElementById(id).addEventListener("click", injectedCode);
	}
}
