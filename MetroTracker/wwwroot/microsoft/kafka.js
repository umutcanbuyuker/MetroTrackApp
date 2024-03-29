"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/ConsumerHub").build();
connection.start().then(function () {
    // Sayfa yüklendiðinde Kafka mesajlarýný al
    connection.invoke("SendMessagesFromProducer").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("KafkaMessages", function (message) {
    success(pos);
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = ` ${message}`;
});