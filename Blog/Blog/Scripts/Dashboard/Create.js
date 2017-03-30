window.onload = function () {

    var date = new Date();
    formattedDate = [
        date.getFullYear(),
        ('0' + (date.getMonth() + 1)).slice(-2),
        ('0' + date.getDate()).slice(-2)
    ].join('-');

    formattedTime = [
        ('0' + date.getHours()).slice(-2),
        ('0' + date.getMinutes()).slice(-2)
    ].join(':');

    var published = document.getElementById("Published");
    published.value = formattedDate + " " + formattedTime;
}