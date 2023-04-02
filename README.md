# Card From
The Card Form library allows your app to process and save credit card information, verify those cards and charge them, with a few lines of code.

You can use the library to create an iframe where none of the raw card data is on your app, all hosted on a gateway server.

### 1) Add the library to your page
```
<script src="/wrapper.js"></script>
```

### 2) Initialize the form

```
const iFrame = new HostedIFrame(sourceKey, mountFormId, mountBtnId);
```

Real World Example:
```
const iFrame = new HostedIFrame(
	"pk_rOYu",
	"iframe",
	"btnSubmit"
);
```

### 2) iFrame.init(); -> The init() method sets everything up and allows for chaining.
#### Returns the Class allowing to set and init() at the same time.
```
iFrame.init();
```
### OR:
```
const iFrame = new HostedIFrame(
	"pk_rOYu",
	"my-div",
	"btnSubmit"
).init();
```

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
	textContent: true, //if true it will add text to the element
};

iFrame.submit(mounts);
```

## Styles:
### iFrame.styles(object); -> Allows for basic styling of the form by taking an object as an argument.
```
const styles = {
	labelType: "floating",
	card: "border-bottom: 1px solid black",
	expiryContainer: "border-bottom: 1px solid black",
	cvv2: "border-bottom: 1px solid black"
};
```
