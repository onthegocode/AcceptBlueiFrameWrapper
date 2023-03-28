let iFrame = new HostedIFrame(
	"pk_kVr1YRC4qMrrOYuuFV10M6VLxXcOp",
	"my-div",
	"btnSubmit"
);

const mounts = {
	form: null,
	token: "hidToken",
	expiryMonth: "hidExpiryMonth",
	expiryYear: "hidExpiryYear",
	cardType: "hidCardType",
	last4: "hidLast4",
	mountError: "test2",
	textContent: true,
};

iFrame.init().submit(mounts);
const styles = {
	labelType: "floating",
	card: "border-bottom: 1px solid black",
	expiryContainer: "border-bottom: 1px solid black",
	cvv2: "border-bottom: 1px solid black",
	avsZip: "border-bottom: 1px solid black",
};
iFrame.styles(styles);
