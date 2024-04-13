# Cross-Language Chat Application

## Description
A powerful multilingual messaging app that brings people together across language barriers. Users can chat seamlessly in their native languages, with real-time translations. It’s like having a personal language interpreter right in your pocket! 🌐🗣️💬🔥

## Features
- **User creation and login**: Users are able to create simple accounts with a username and a password. There is basic authentication that checks if the username and password are valid before allowing application usage.
- **Chat room management**: Users can create and delete chat rooms. There is a left sidebar that allows users to manage all of their active chat rooms.
- **Adding users to chat rooms**: Users that create their own chat rooms can add as many other users to their chat rooms and receive messages from one another.
- **Language translations**: Messages can be sent in the users' native languages.  They will be translated to the receiving users' native languages.

## Technologies
### Framework
- **ASP.NET**

### Frontend
- **HTML**, **CSS**, **JavaScript**

### Backend
- **C#**, **PostgreSQL**

### External API
- **DeepL Translate API**

## Methodology
### Framework
- **ASP.NET**
    - ASP.NET defined the Model-View-Controller (MVC) design pattern for the entirety of the application.
    - Chat room, user, and message models were defined as data models for how they would stored in the database.
    - Views were the UI components and would be served via server-side rendering.
    - Controllers handled user input, such as sending messages, by passing data through forms and processing them through the RESTful API.

### Frontend
- **HTML**
    - HTML and CSS form the foundation for the user interface and visual components.
    - The login page, landing page, and various forms were developed using HTML.
    - CSS was used to style these components, ensuring a cohesive and visually appealing design.

- **JavaScript**
    - JavaScript played a crucial role in client-side interactions.
    - Key functionalities included:
        - Creating chat rooms dynamically.
        - Navigating between existing chat rooms.
        - Adding users to chat rooms.

### Backend
- **C#**
    - C# was the language of choice for creating a RESTful API.
    - Server-side actions and rendering were handled using C#.
    - CRUD operations related to chat rooms, user management, and message handling were implemented.
    - Dependency injection facilitated loose coupling with the database.

- **PostgreSQL**
    - PostgreSQL served as the database.
    - It stored information such as user profiles, chat room details, and messages.
    - The relational nature of PostgreSQL ensured seamless connections between users and the chat rooms they participated in.

### External API
- **DeepL Translate API**
    - DeepL Translate API handled message translations.
    - Integrated the API into a translation method that took a message as a string and returned the translated message as output.

## Screenshots
