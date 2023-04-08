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
};

const iFrame = new HostedIFrame("pk_rXY", "iframeMount").styles(styles);

$('#btnSubmit').click(() => {
    iFrame.submit(dataObj).then(response => {
        token = response.token;
        console.log(response);
    }).catch(response => {
        document.querySelector('#errorMount').textContent = ("" + response).replace("Error: ", "");
        throw new Error(("" + response).replace("Error: ", ""));
    })
});
