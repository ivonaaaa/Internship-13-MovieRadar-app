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
      try {
        const errorData = JSON.parse(error.message);
        if (errorData.errors) {
          const messages = Object.values(errorData.errors).flat().join("\n");
          alert(messages);
          return;
        }
      } catch {
        alert(error.message || "An error occurred.");
      }
    }
  });
});
