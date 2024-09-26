CREATE DOMAIN vkUsername AS TEXT
CHECK(
   VALUE ~ '(^\d{1,2}[a-zA-Z_]+(\.[a-zA-Z_]{1}[a-zA-Z0-9_]{2,})*[a-zA-Z0-9_]$)|(^[a-zA-Z_]+[a-zA-Z0-9_]*(\.[a-zA-Z_]{1}[a-zA-Z0-9_]{2,})*[a-zA-Z0-9]$)'
);

CREATE DOMAIN tgUsername AS TEXT
CHECK(
   VALUE ~ '^[a-zA-Z]+[a-zA-Z0-9_]*[a-zA-Z0-9]$'
);

CREATE DOMAIN phoneNumberRu AS TEXT
CHECK(
   VALUE ~ '^\+7\(\d{3}\)\s\d{3}-\d{2}-\d{2}$'
);

CREATE DOMAIN emailAddress AS TEXT
CHECK(
   VALUE ~ '^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$'
);