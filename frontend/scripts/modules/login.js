import { initAdminApp } from "./admin-app.js";
import { initUserApp } from "./user-app.js";
import { getAllUsers } from "../api/api.js";

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
      const allUsers = await getAllUsers();

      const user = allUsers.find(
        (u) => u.email === email && u.password === password
      );
      if (user) {
        loginContainer.style.display = "none";
        try {
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
      if (registerContainer) registerContainer.style.display = "block";
    });
  }
});
