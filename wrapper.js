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
function charge(token) {
	$(document).ready(function () {
		$.ajax({
			url: "/api/transactions/sourcecharge",
			contentType: "application/json",
			data: JSON.stringify({ Source: token }),
			type: "POST",
			success: function (data) {
				console.log(data);
			},
			error: function (error) {
				console.log(error);
			},
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
			// const resultMount = this._resultMount; //set outside the promise to access HostedIFrame
			const errorResult = this._errorMount; //set outside the promise to access HostedIFrame
			const sourceVerification = this._sourceVerification; //set outside the promise to access HostedIFrame
			this.cardForm
				.getNonceToken()
				.then((result) => {
					//calls the api to verify nonce token and returns token
					sourceVerification({
						Source: "nonce-" + result.nonce,
						Expiry_Month: result.expiryMonth,
						Expiry_Year: result.expiryYear,
					});

					/*_getAndSet({ [submitMounts.form]: { submit: true } });*/ //submits the form
				})
				.catch((mainError) => {
					let error = ("" + mainError).replace("Error: ", "");
					errorResult(submitMounts.mountError, error, submitMounts.textContent);
					throw new Error(error);
				});
		});
		return this;
	}
	//used to preset styles based on an object that will be provided with multiple items within that will change the style
	styles(styles) {
		this._onLoad(() => this.cardForm.setStyles(styles));
		return this;
	}

	//Allows you to choose which element you'd want to mount the error to. Can be a value or set as textContent
	_errorMount(errorMount, _mainError, textContent = false) {
		_getAndSet({ [errorMount]: { value: _mainError, text: textContent } });
	}

	//The result mount is used to mount the nonce token, expiry dates, last 4, and card type. But may not have a function in the grand scheme of things
	/*_resultMount(
		obj,
		result,
		formMount = null,
		tokenMount = null,
		expiryMonthMount = null,
		expiryYearMount = null,
		cardTypeMount = null,
		last4Mount = null
	) {
		_getAndSet({
			[tokenMount]: { value: result.nonce },
			[expiryMonthMount]: { value: result.expiryMonth },
			[expiryYearMount]: { value: result.expiryYear },
			[cardTypeMount]: { value: result.cardType },
			[last4Mount]: { value: result.last4 },
			[formMount]: { submit: true },
		});
	}*/


	//Checks to see if the page was loaded before running a function
	//takes a function as a parameter
	_onLoad(injectedCode) {
		window.addEventListener("load", injectedCode);
	}

	//event listener to see if the element based on Id was clicked or not
	_clicked(id, injectedCode) {
		document.getElementById(id).addEventListener("click", injectedCode);
	}

	//Calls the SourceVerification Method from the VerificationController takes an object as a parameter and returns a token that can be stored in the customer profile and later be charged
	_sourceVerification(dataObj) {
		$(document).ready(function () {
			$.ajax({
				url: "/api/transactions/sourceverification",
				contentType: "application/json",
				data: JSON.stringify(dataObj),
				type: "POST",
				success: function (data) {
					charge(data.card_Ref); //just here for testing purposes
				},
				error: function (error) {
					console.log("Error:");
					console.log(error);
				},
			});
		});
	}
}
