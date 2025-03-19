let InputSearch = document.getElementById("SearchInput");

InputSearch.addEventListener("keyup", function () {
    let xhr = new XMLHttpRequest();

    // Use backticks for template literals
    let url = `https://localhost:44307/Employee?SearchInput=${InputSearch.value}`;

    xhr.open("GET", url, true);

    xhr.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            console.log(this.responseText); // Fixed case-sensitive error
        }
    };

    xhr.send();
});
