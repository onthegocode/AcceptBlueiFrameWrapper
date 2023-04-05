# Card Form:

The Card Form library allows your app to process and save credit card information, verify those cards and charge them, with a few lines of code.

You can use the library to create an iframe where none of the raw card data is on your app, all hosted on a gateway server.

## Set Up:

### 1) Add script Tag 

```
<script src="./wrapper.js"></script>
```
### 2) Create and Initialize the Form

```
const iFrame = new HostedIFrame("pk_rIY", "iframeMount");
```

### 3) Submit and recieve reponse -> Token

The submit method will be called returning a promise that you can capture the JSON response using .then() and catch any errors using .catch(). A Token will be returned in the JSON response allowing you to charge that token later on. Make sure to wrap the .submit() method inside a click event. 

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
  status: "Approved",
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


## Style:



## Charge:
This function allows you to charge token that you get from the submit() method. It takes an object as a parameter where the Amount and Source are required. But you have other options such as Avs_Address, Avs_Zip, CVV2, Expiry_Month, Expiry_Year, and Software (These options are optional).

## Sample Code:
```
<div style="display: flex; flex-direction: column; gap: 6rem;">
	<form id="formMount">
		<div id="iframeMount"></div>
		<p id="errorMount"></p>
		<button type="button" id="btnSubmit">Submit</button>
	</form>

	<form id="chargeForm">
		<label style="display:block;" for="amount">Amount:</label>
		<input id="amount" name="amount" type="number" required />
		
		<button type="button" id="btn2">Charge Card</button>
	</form>
</div>

<script type="text/javascript">
    let token;
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
        Avs_Zip: "53402",
        Software: "Example Software",
    };

    const iFrame = new HostedIFrame("PUBLIC SOURCE KEY", "iframeMount").styles(styles);

    $('#btnSubmit').click(() => {
        iFrame.submit(dataObj).then(response => {
            token = response.token;
            console.log(response);
        }).catch(response => {
            throw new Error(("" + response).replace("Error: ", ""));
        })
    });

    $("#btn2").click(() => {
        let chargeObj = {
            Source: token,
            Amount: parseFloat($("#amount").val()),
        };

        charge(chargeObj).then(response => {
            console.log(response);
        }).catch(msg => {
            throw new Error(("" + msg).replace("Error: ", ""));
        });
    });
</script>
```
