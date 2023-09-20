import React, { useState, useEffect } from "react";
import "./register.css";
import { Link, useNavigate } from "react-router-dom";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import Cookies from "js-cookie";
function Register() {
  const navigate = useNavigate();
  useEffect(() => {
    const isLogin = Cookies.get("isLogin");
    if (isLogin) {
      navigate("/mainpage");
    }
  });

  const [email, setUsername] = useState("");
  const [pwd, setPassword] = useState("");
  const [phone, setPhone] = useState("");
  const [birthdate, setBirthdate] = useState(null);
  const [firstname, setFname] = useState("");
  const [lastname, setLname] = useState("");

  const inputEmail = (e) => {
    setUsername(e.target.value);
  };
  const inputPassword = (e) => {
    setPassword(e.target.value);
  };
  const inputFname = (e) => {
    setFname(e.target.value);
  };
  const inputLname = (e) => {
    setLname(e.target.value);
  };
  const inputPhone = (e) => {
    setPhone(e.target.value);
  };
  const inputBirthdate = (date) => {
    setBirthdate(date);
  };

  const register = async (event) => {
    event.preventDefault();

    try {
      var formData = {
        email: email,
        password: pwd,
        firstName: firstname,
        lastName: lastname,
        phoneNumber: phone,
        brithdate: birthdate,
      };
      console.log("formData :>> ", formData);
      const response = await fetch(
        "https://localhost:7263/api/Authrorizations/Sigup",
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
        console.error("Register failed");
      }
    } catch (error) {
      console.error("Error:", error);
    }
  };
  return (
    <div className="register-container">
      <div className="register-card">
        <h1>Register</h1>
        <div className="form-group">
          <label htmlFor="email">Email</label>
          <input
            onChange={inputEmail}
            type="email"
            id="email"
            name="email"
            placeholder="Enter your email"
          />
        </div>
        <div className="form-group">
          <label htmlFor="password">Password</label>
          <input
            onChange={inputPassword}
            type="password"
            id="password"
            name="password"
            placeholder="Enter your password"
          />
        </div>
        <div className="form-group">
          <label htmlFor="password">Firstname</label>
          <input
            onChange={inputFname}
            type="text"
            id="fname"
            name="fname"
            placeholder="Enter your firstname"
          />
        </div>
        <div className="form-group">
          <label htmlFor="password">Lastname</label>
          <input
            onChange={inputLname}
            type="text"
            id="lname"
            name="lname"
            placeholder="Enter your lastname"
          />
        </div>
        <div className="form-group">
          <label htmlFor="birthdate">Birthdate</label>
          <DatePicker
            selected={birthdate}
            onChange={inputBirthdate}
            dateFormat="yyyy/MM/dd"
            isClearable={true}
            placeholderText="Select birthdate"
            className="custom-datepicker"
          />
        </div>
        <div className="form-group">
          <label htmlFor="phone">Mobile phone number</label>
          <input
            onChange={inputPhone}
            type="number"
            id="phone"
            name="phone"
            placeholder="Enter your mobile phone number"
          />
        </div>
        <button className="primary-button" onClick={register}>
          Register
        </button>
        <br></br>
        <Link to="/">
          <button type="submit" className="normal-button mt-10">
            Cancel
          </button>
        </Link>
      </div>
    </div>
  );
}

export default Register;
