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

### 3) Submit and recieve reponse

On button clicked the submit method will be called returning a promise that you can capture the JSON response using .then() and catch any errors using .catch(). 

```
$('#btnSubmit').click(() => {
  iFrame.submit(dataObj).then(response => {
      token = response.token;
      console.log(response);
  }).catch(response => {
      throw new Error(("" + response).replace("Error: ", ""));
  })
});
```
Json Verification And Save Response Example:
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



## Charge

## Sample Code:
