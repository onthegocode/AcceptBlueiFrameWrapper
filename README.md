# Card From
The Card Form library allows your app to process and save credit card information, verify those cards and charge them, with a few lines of code.

You can use the library to create an iframe where none of the raw card data is on your app, all hosted on a gateway server.

##Form Setup:

### 1) Add the library to your page

```
<script src="/wrapper.js"></script>
```

### 2) Initialize the form

#### a) Setup:

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
#### b) Init and Submit:

init(): Initializes the form

```
iFrame.init();
```

submit(): Takes an object that allows the user to select the form, in which they want to submit and mount where the error should show if the user wants. Also allowing for error to be shown as text by setting textContent to true.

```
const mounts = {
  form: "formMount",
  mountError: "errorMount",
  textContent: true,
};

iFrame.submit(mounts);
```


