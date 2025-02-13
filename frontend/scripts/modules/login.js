import { saveAuthToken, decodeToken } from "./auth.js";
import { initAdminApp } from "./admin-app.js";
import { initUserApp } from "./user-app.js";
import { loginUser, getUserById } from "../api/api.js";

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
      const data = await loginUser(email, password);
      const token = data.token;

      saveAuthToken(token);

      loginContainer.style.display = "none";

      const decoded = decodeToken(token);
      const userId = decoded ? decoded.sub : null;
      if (!userId) {
        throw new Error("Failed to retrieve user ID from token.");
      }

      const user = await getUserById(userId);

      // Provjera je li korisnik admin
      if (user.isAdmin) {
        initAdminApp();
      } else {
        initUserApp();
      }
    } catch (error) {
      alert(error.message);
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
