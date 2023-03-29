# AcceptBlueiFrameWrapper
## Card From:
### 1) const iFrame = new HostedIFrame(sourceKey, mountFormId, mountBtnId);

Example:
```
const iFrame = new HostedIFrame(
	"pk_rOYu",
	"my-div",
	"btnSubmit"
);
```

### 2) iFrame.init(); -> The init() method sets everything up and allows for chaining.

### 3) iFrame.submit(object); -> The submit() method mounts to a button and returns the nonce token, expirymonth, expiryyear, cardtype, and last4 
#### Must mount the nonce token, expirymonth, expiryyear, cardtype, and last4. Takes an object as an argument.
```
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

iFrame.submit(mounts);
```
