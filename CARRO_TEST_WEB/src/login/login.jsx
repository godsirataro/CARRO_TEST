import React, { useState, useEffect } from "react";
import "./login.css";
import { Link, useNavigate } from "react-router-dom";
import Cookies from "js-cookie";

function Login() {
  const navigate = useNavigate();
  useEffect(() => {
    const isLogin = Cookies.get("isLogin");
    if (isLogin) {
      navigate("/mainpage");
    }
  });




  const [email, setUsername] = useState("");
  const [pwd, setPassword] = useState("");

  const inputEmail = (e) => {
    setUsername(e.target.value);
  };
  const inputPwd = (e) => {
    setPassword(e.target.value);
  };

  const login = async (event) => {
    event.preventDefault();
    try {
      var formData = {
        email: email,
        password: pwd,
      };
      const response = await fetch(
        "https://localhost:7263/api/Authrorizations/Sigin",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            XApiKey: "}-Yy1:N2_*T:r!Y:wbE%XzFD?2@A{M/Q",
          },
          body: JSON.stringify(formData),
        }
      );
     
      if (response.status == 200) {
        Cookies.set("isLogin", "true");
      window.location.reload();

      } else {
        console.error("Login failed");
      }
    } catch (error) {
      console.error("Error:", error);
    }
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <h1>Login</h1>
        <div className="form-group">
          <label htmlFor="email">Email</label>
          <input
            type="text"
            id="email"
            name="email"
            placeholder="Enter your email"
            onChange={inputEmail}
          />
        </div>
        <div className="form-group">
          <label htmlFor="password">Password</label>
          <input
            type="password"
            id="password"
            name="password"
            onChange={inputPwd}
            placeholder="Enter your password"
          />
        </div>
        <button className="primary-button" onClick={login}>
          Login
        </button>
        <br></br>
        <Link to="/">
          <button className="normal-button mt-10">Cancel</button>
        </Link>
        <br></br>
        <Link to="/register" className="register-button">
          create new account
        </Link>
      </div>
    </div>
  );
}

export default Login;
