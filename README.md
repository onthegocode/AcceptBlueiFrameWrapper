# Card From
The Card Form library allows your app to process and save credit card information, verify those cards and charge them, with a few lines of code.

You can use the library to create an iframe where none of the raw card data is on your app, all hosted on a gateway server.

## Form Setup:

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

## Styling:
Set styles for the elements inside the card form/iframe, by using the styles() method. The styles method takes an object as a parameter.

```
const styles = {
   labelType: "floating",
   card: "border-bottom: 1px solid black",
   expiryContainer: "border-bottom: 1px solid black",
   cvv2: "border-bottom: 1px solid black",
   avsZip: "border-bottom: 1px solid black",
};

iFrame.styles(styles);
```

## Real World Example:
```
const mounts = {
   form: "formMount",
   mountError: "errorMount",
   textContent: true,
};

const styles = {
   labelType: "floating",
   card: "border-bottom: 1px solid black",
   expiryContainer: "border-bottom: 1px solid black",
   cvv2: "border-bottom: 1px solid black",
   avsZip: "border-bottom: 1px solid black",
};

new HostedIFrame("pk_kVr1YRC4qMrrOYuuFV10M6VLxXcOp", "iframeMount", "btnSubmit")
   .init()
   .styles(styles)
   .submit(mounts);
```
