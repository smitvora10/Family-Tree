---
description: Test User Registration Flow via Swagger
---

1. **Open Swagger UI**
   - Navigate to `http://localhost:5288/swagger/index.html`

2. **Locate Endpoint**
   - Expand the `CLAuth` controller section.
   - Expand the `POST /api/Auth/register` endpoint.

3. **Prepare Request**
   - Click the `Try it out` button.
   - In the Request body, use the following JSON template:
     ```json
     {
       "fullName": "Test User",
       "username": "testuser_unique_1",
       "email": "recipient@example.com",
       "password": "Password123!",
       "mobileNumber": "1234567890",
       "userRoleId": 1
     }
     ```
   - **Note**: Ensure `username` and `email` are unique if testing multiple times (unless cleaning DB).
   - **Note**: `mobileNumber` accepts any string format (validation is relaxed).

4. **Execute**
   - Click the blue `Execute` button.

5. **Verify Response**
   - **Success (200 OK)**:
     ```json
     {
       "isError": false,
       "message": "Registration successful. OTP sent to the registered email address.",
       ...
     }
     ```
   - **Failure**: Check the `message` field for validation errors (e.g., "Username already exists").

6. **Verify Email**
   - Check the inbox of the `email` provided in the request for the OTP code.
