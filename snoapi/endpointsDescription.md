# Endpoints usage

- GET /api/events: get all events
    - /api/events/{id}: get single event with specified id

- GET /api/projects: get all projects
    - /api/projects/{id}: get single project with specified id
        - /api/projects/{id}/authors: get authors of specified project (with its id)

- GET /api/users: get all users
    - /api/users/{id}: get single user with specified id
        - /api/users/{id}/projects: get projects of specified author (with author`s id)
- GET /api/skills: get all skills of all departments
