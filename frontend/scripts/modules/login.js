
import { generateJWT, saveAuthToken } from "./auth.js";
import { mockUsers } from "../../marul-test/marul-mock-data.js";

document.addEventListener("DOMContentLoaded", function () {
  const loginContainer = document.getElementById("login-container");
  if (!loginContainer) return; 

  
  const loginButton = loginContainer.querySelector("button");
  loginButton.addEventListener("click",async function () {
    const usernameInput = loginContainer.querySelector("input[placeholder='Username']");
    const passwordInput = loginContainer.querySelector("input[placeholder='Password']");
    const username = usernameInput.value.trim();
    const password = passwordInput.value.trim();

    if (!username || !password) {
      alert("Please enter username and password.");
      return;
    }
    try {
      // Dohvaćamo sve korisnike s backend-a
      const response = await fetch("http://localhost:50845/api/User", {
        method: "GET",
        headers: {
          "Content-Type": "application/json"
        }
      });
      
      if (!response.ok) {
        throw new Error(`Error fetching users: ${response.statusText}`);
      }

      const allUsers = await response.json();

      // Pretražujemo korisnike i provjeravamo postoji li onaj s istim username i password
      const user = allUsers.find(u => u.username === username && u.password === password);
      if (user) {
        const token = generateJWT(user);
        saveAuthToken(token);
        // Preusmjeravamo na landing page
        window.location.href = "ivona-test.html";
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
