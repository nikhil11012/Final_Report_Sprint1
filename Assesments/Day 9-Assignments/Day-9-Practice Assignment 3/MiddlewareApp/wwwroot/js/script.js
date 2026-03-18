// script.js - JavaScript file for the Middleware Application

// This script runs when the page loads and updates the message on the page
document.addEventListener("DOMContentLoaded", function () {
    var messageElement = document.getElementById("js-message");
    messageElement.textContent = "JavaScript is working! This message was added dynamically.";
});
