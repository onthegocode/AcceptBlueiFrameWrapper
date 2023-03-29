const mounts = {
	form: "frmTest",
	token: "hidToken",
	expiryMonth: "hidExpiryMonth",
	expiryYear: "hidExpiryYear",
	cardType: "hidCardType",
	last4: "hidLast4",
	mountError: "test2",
	textContent: true,
};

const styles = {
	labelType: "floating",
	card: "border-bottom: 1px solid black",
	expiryContainer: "border-bottom: 1px solid black",
	cvv2: "border-bottom: 1px solid black",
	avsZip: "border-bottom: 1px solid black",
};

const iFrame = new HostedIFrame("pk_rOYu", "my-div", "btnSubmit")
	.init()
	.styles(styles)
	.submit(mounts);
