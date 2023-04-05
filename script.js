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

const iFrame = new HostedIFrame("pk_kVr1YRC4qMrrOYuuFV10M6VLxXcOp", "iframeMount").styles(styles)

$('#btnSubmit').click(() => {
    iFrame.submit(dataObj).then(msg => {
        token = msg.token;
        console.log(msg);
    }).catch(msg => {
        throw new Error(("" + msg).replace("Error: ", ""));
    })

});

$("#btn2").click(() => {
    let newObj = {
        Source: token,
        Amount: parseFloat($("#amount").val()),
    };
    charge(newObj).then(msg => {
        console.log(msg);
    }).catch(msg => {
        throw new Error(("" + msg).replace("Error: ", ""));
    });
});
