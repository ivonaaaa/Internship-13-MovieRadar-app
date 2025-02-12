import { generateJWT, saveAuthToken } from "./auth.js";
import { mockUsers } from "../../marul-test/marul-mock-data.js";
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
    const username = usernameInput.value.trim();
    const password = passwordInput.value.trim();

    if (!username || !password) {
      alert("Molimo unesite korisničko ime i lozinku.");
      return;
    }

    let registeredUsers =
      JSON.parse(localStorage.getItem("registeredUsers")) || [];
    const allUsers = [...mockUsers, ...registeredUsers];

    //test sa mock podacima
    const user = allUsers.find(
      (u) => u.username === username && u.password === password
    );
    if (user) {
      const token = generateJWT(user);
      saveAuthToken(token);

      //! ode treba pozvat ralizite funkcije ovisno o tome je li admin ili ne
      loginContainer.style.display = "none";
      try {
        if (user.admin) {
          initAdminApp();
        } else {
          initUserApp();
        }
      } catch (error) {
        console.error("Greška pri učitavanju modula:", error);
      }
    } else {
      alert("Neispravno korisničko ime ili lozinka.");
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
