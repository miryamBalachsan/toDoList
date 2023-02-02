
function Login() {
    const name = document.getElementById('add-name');
    const password = document.getElementById('add-password');
    var myHeaders = new Headers();

    myHeaders.append("Content-Type", "application/json");
    var raw = JSON.stringify({
        Id: 0,
        Name: name.value.trim(),
        IsAdmin: false,
        Password: password.value.trim()
    })
    const a=name.value.trim();
    const b=password.value.trim();

    fetch("User/Login",{
        method: "POST",
        headers: myHeaders,
        body: raw,
        redirect: "follow",
    })
        .then((response) => response.text())
        .then((result) => {
            if (result.includes("401")) {      
                name.value = "";
                password.value = "";
                alert("not exist!!")
            }
             else {
                token = result;
                // alert(token);
                sessionStorage.setItem("token", token)             
                location.href="task.html";

            }
        }).catch((error) => alert("error"+error));

}