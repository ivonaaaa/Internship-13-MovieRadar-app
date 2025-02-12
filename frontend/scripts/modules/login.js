import { saveAuthToken } from "./auth.js";
import { initAdminApp } from "./admin-app.js";
import { initUserApp } from "./user-app.js";

document.addEventListener("DOMContentLoaded", function () {
  const loginContainer = document.getElementById("login-container");
  if (!loginContainer) return;

  const loginButton = loginContainer.querySelector("button");
  loginButton.addEventListener("click", async function () {
    const usernameInput = loginContainer.querySelector(
      "input[placeholder='Username']"
    );
    const passwordInput = loginContainer.querySelector(
      "input[placeholder='Password']"
    );
    const email = usernameInput.value.trim();
    const password = passwordInput.value.trim();

    if (!email || !password) {
      alert("Please enter email and password.");
      return;
    }

    try {
      // Dohvaćamo sve korisnike iz baze
      const response = await fetch("https://localhost:50844/api/User", {
        method: "GET",
        headers: {
          "Content-Type": "application/json"
        }
      });
      if (!response.ok) {
        throw new Error(`Error fetching users: ${response.statusText}`);
      }
      const allUsers = await response.json();

      // Tražimo korisnika s odgovarajućim podacima
      const user = allUsers.find(u => u.email === email && u.password === password);
      if (user) {
        loginContainer.style.display = "none";
        try {
          // Pozivamo funkcije ovisno o tome je li korisnik admin
          if (user.is_admin) {
            initAdminApp();
          } else {
            initUserApp();
          }
        } catch (error) {
          console.error("Error while loading module:", error);
        }
      } else {
        alert("Invalid username or password.");
      }
    } catch (error) {
      console.error("An error occurred:", error);
      alert("An error occurred while accessing the server.");
    }
  });

  const createBtn = document.getElementById("create-btn");
  if (createBtn) {
    createBtn.addEventListener("click", function () {
      loginContainer.style.display = "none";
      const registerContainer = document.getElementById("register-container");
      if (registerContainer) {
        registerContainer.style.display = "block";
      }
    });
  }
});
