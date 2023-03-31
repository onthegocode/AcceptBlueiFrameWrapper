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

//Ajax Functions
function charge(data) {
	$(document).ready(function () {
		$("#btnOkSourcecharge").click(function () {
			$.ajax({
				url: "/api/transactions/Sourcecharge",
				beforeSend: function () {
					$("#btnCancelSourcecharge").attr("disabled", true);
					$("#btnOkSourcecharge").attr("disabled", true);
				},
				contentType: "application/json",
				data: JSON.stringify(data),
				type: "POST",
				success: function (data) { },
				error: function (error) { },
			});
		});
	});
}

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
			const resultMount = this._resultMount; //set outside the promise to access HostedIFrame
			const errorResult = this._errorMount; //set outside the promise to access HostedIFrame
			const sourceVerification = this._sourceVerification;
			this.cardForm
				.getNonceToken()
				.then((result) => {
					sourceVerification({
						Source: 'nonce-' + result.nonce,
						Expiry_Month: result.expiryMonth,
						Expiry_Year: result.expiryYear,
					});
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
		return this;
	}
	//used to preset styles based on an object that will be provided with multiple items within that will change the style
	styles(styles) {
		this._onLoad(() => this.cardForm.setStyles(styles));
		return this;
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
	//Ajax function that does a source verification and returns a token to later be stored in the customers profile
	_sourceVerification(dataObj) {
		$(document).ready(function () {
			$.ajax({
				url: "/api/transactions/sourceverification",
				contentType: "application/json",
				data: JSON.stringify(dataObj),
				type: "POST",
				success: function (data) {
					console.log("Success:");
					console.log(data);
				},
				error: function (error) {
					console.log("Error:");
					console.log(error);
				},
			});
		});
	}
}
