const url = '/User';
let token = sessionStorage.getItem("token");
function getUsers() {
    fetch(url, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        }
    })
        .then(response => response.json())
        .then(result => { _displayItems(result) })
        .catch(error => console.log('error', error));
}
const _displayItems = (data) => {
    const tBody = document.getElementById('items');
    tBody.innerHTML = '';
    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isDoCheckbox = document.createElement('input');
        isDoCheckbox.type = 'checkbox';
        isDoCheckbox.disabled = true;
        isDoCheckbox.checked = item.isAdmin

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Update';
        editButton.setAttribute('onclick', `displayUpdateForm(${item.Id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteUser(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isDoCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);


        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    items = data;
}
const _displayCount = (itemCount) => {
    const name = (itemCount === 1) ? 'task' : 'tasks kinds';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}
function addUser() {
    const addNameTextbox = document.getElementById('add-name');
    const user = {
      Name: addNameTextbox.value.trim()
    };
    fetch(url, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
        
        body: JSON.stringify(user)
    })
        .then(response => response.json())
        .then(() => {
            getUsers();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}



function deleteUser(id) {
    alert(id)
     fetch(`${url}/${id}`, {
         method: 'DELETE',
         headers: {
             'Accept': 'application/json',
             'Content-Type': 'application/json',
             'Authorization': 'Bearer ' + sessionStorage.getItem("token")
         },
     })
         .then(() => getUsers())
         .catch(error => console.log('Unable to delete item.', error));
 }