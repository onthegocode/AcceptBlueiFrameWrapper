# Card Form:

The Card Form library allows your app to process and save credit card information, verify those cards and charge them, with a few lines of code.

You can use the library to create an iframe where none of the raw card data is on your application, all hosted on a gateway server.

## Set Up:

### 1) Add script Tag 

```
<script src="./wrapper.js"></script>
```
### 2) Create and Initialize the Form

```
const iFrame = new HostedIFrame("pk_rIY", "iframeMount");
```

### 3) Submit and receive reponse -> Token

The submit method will be called returning a promise that you can capture the JSON response using .then() and catch any errors using .catch(). A Token will be returned in the JSON response allowing you to charge that token at a later date. Make sure to wrap the .submit() method inside a click event. 

It takes an object as a parameter but is only needed if you'd like Name, Avs_Address, Avs_Zip, and Software to be returned to you in JSON response.

#### Example Object:
```
const dataObj = {
    Name: "Sam",
    Avs_Address: "78 Boston Court Racine, WI",
    Avs_Zip: "53402",
    Software: "Example Software",
};
```


#### Example Submit: 
Without click event
```
iFrame.submit(dataObj).then(response => {
    console.log(response);
}).catch(response => {
    throw new Error(("" + response).replace("Error: ", ""));
});
```
#### Json Response Example:
```
{
  status: "Success",
  token: "DNJMJOZMRAUG2G6K",
  card_Type: "Visa",
  last_4: "1118",
  expiry_Month: 12,
  expiry_Year: 2026,
  name: "Sam",
  avs_Address:"78 Boston Court Racine, WI",
  avs_Zip : "53402",
  software: "Example Software",
  error_Details: null,
  error_Message: null
}
```

## Sample Code:
In this example, we are verifying and saving the credit card as a token, it will return a JSON response containing a Token. In this example the JSON response is logged to the console. Also in this example errors will be catched and thrown. Card form errors will appear as text.

Please provide your own Public Source Key.

```
<div style="display: flex; flex-direction: column; gap: 6rem;">
	<form id="formMount">
		<div id="iframeMount"></div>
		<p id="errorMount"></p>
		<button type="button" id="btnSubmit">Submit</button>
	</form>
</div>

<script type="text/javascript">
	const styles = {
            labelType: "floating",
            card: "border-bottom: 1px solid black",
            expiryContainer: "border-bottom: 1px solid black",
            cvv2: "border-bottom: 1px solid black",
            avsZip: "border-bottom: 1px solid black",
        };

        const dataObj = {
            Name: "Sam",
            Avs_Address: "78 Boston Court Racine, WI",
	    Avs_Zip: "56932",
	    Software: "Example Software",
        };

        const iFrame = new HostedIFrame("PUBLIC SOURCE KEY", "iframeMount").styles(styles);

        $('#btnSubmit').click(() => {
            iFrame.submit(dataObj).then(response => {
                console.log(response);
            }).catch(response => {
                document.querySelector('#errorMount').textContent = ("" + response).replace("Error: ", "");
                throw new Error(("" + response).replace("Error: ", ""));
            })
        });
</script>
```
