import { initUserApp } from "./user-app.js";
document.addEventListener("DOMContentLoaded", function () {
  const registerContainer = document.getElementById("register-container");
  if (!registerContainer) return;

  const registerButton = registerContainer.querySelector("button");
  registerButton.addEventListener("click", async function () {
    const firstNameInput = registerContainer.querySelector("input[placeholder='FirstName']");
    const lastNameInput = registerContainer.querySelector("input[placeholder='LastName']");
    const emailInput = registerContainer.querySelector("input[placeholder='Email']");
    const passwordInput = registerContainer.querySelector("input[placeholder='Password']");

    const firstName = firstNameInput.value.trim();
    const lastName = lastNameInput.value.trim();
    const email = emailInput.value.trim();
    const password = passwordInput.value.trim();

    if (!firstName || !lastName || !email || !password) {
      alert("All fields are required.");
      return;
    }

    try {
      // Provjera postoji li već korisnik s tim emailom
      const getResponse = await fetch("https://localhost:50844/api/User", {
        method: "GET",
        headers: {
          "Content-Type": "application/json"
        }
      });
      if (!getResponse.ok) {
        throw new Error(`Error fetching users: ${getResponse.statusText}`);
      }
      const allUsers = await getResponse.json();
      const emailExists = allUsers.some(u => u.email === email);
      if (emailExists) {
        alert("A user with this email already exists.");
        return;
      }

      // Kreiranje novog korisnika
      const newUser = {
        firstName,
        lastName,
        email,
        password,
        is_admin: false
      };

      const postResponse = await fetch("https://localhost:50844/api/User", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(newUser)
      });

      if (!postResponse.ok) {
        throw new Error(`Error creating user: ${postResponse.statusText}`);
      }

      registerContainer.style.display = "none";
      try {
        initUserApp();
      } catch (error) {
        console.error("Error while loading user app:", error);
      }

    } catch (error) {
      console.error("An error occurred during registration:", error);
      alert("An error occurred while accessing the server.");
    }
  });
});
