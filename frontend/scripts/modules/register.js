import { initUserApp } from "./user-app.js";
import { registerUser } from "../api/api.js";

document.addEventListener("DOMContentLoaded", function () {
  const registerContainer = document.getElementById("register-container");
  if (!registerContainer) return;

  const registerButton = registerContainer.querySelector("button");
  registerButton.addEventListener("click", async function () {
    const firstNameInput = registerContainer.querySelector(
      "input[placeholder='FirstName']"
    );
    const lastNameInput = registerContainer.querySelector(
      "input[placeholder='LastName']"
    );
    const emailInput = registerContainer.querySelector(
      "input[placeholder='Email']"
    );
    const passwordInput = registerContainer.querySelector(
      "input[placeholder='Password']"
    );

    const firstName = firstNameInput.value.trim();
    const lastName = lastNameInput.value.trim();
    const email = emailInput.value.trim();
    const password = passwordInput.value.trim();

    if (!firstName || !lastName || !email || !password) {
      alert("All fields are required.");
      return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
      alert("Please enter a valid email address.");
      return;
    }

    if (password.length < 8) {
      alert("Password must be at least 8 characters long.");
      return;
    }

    if (!/\d/.test(password)) {
      alert("Password must contain at least one number.");
      return;
    }

    if (!/[A-Z]/.test(password)) {
      alert("Password must contain at least one uppercase letter.");
      return;
    }

    if (!/[a-z]/.test(password)) {
      alert("Password must contain at least one lowercase letter.");
      return;
    }

    if (!/[!@#$%^&*(),.?":{}|<>]/.test(password)) {
      alert(
        'Password must contain at least one special character (!@#$%^&*(),.?":{}|<>).'
      );
      return;
    }

    try {
      const newUser = {
        firstName,
        lastName,
        email,
        password,
      };

      await registerUser(newUser);

      alert("Registration successful, please login now");
      window.location.href = "./index.html";
    } catch (error) {
      alert(error.message || "An error occurred during registration.");
    }
  });
});
