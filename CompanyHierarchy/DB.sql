DROP TABLE IF EXISTS Employees;

CREATE TABLE Employees (
    EmployeeId SERIAL PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Title VARCHAR(100) NOT NULL,
    ManagerEmployeeId INTEGER NULL,
    FOREIGN KEY (ManagerEmployeeId) REFERENCES Employees(EmployeeId)
);

CREATE INDEX idx_employees_manageremployeeid ON Employees(ManagerEmployeeId);
CREATE INDEX idx_employees_manageremployeeid_employeeid ON Employees(ManagerEmployeeId, EmployeeId);

-- Insert top-level managers (CEO and CTO)
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('Alice Johnson', 'CEO', NULL);
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('Bob Smith', 'CTO', NULL);

-- Insert employees under CEO
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('Carol Williams', 'CFO', 1);
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('David Brown', 'COO', 1);

-- Insert employees under CTO
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('Eve Davis', 'VP of Engineering', 2);
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('Frank Wilson', 'VP of Product', 2);

-- Insert employees under CFO
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('Grace Miller', 'Finance Manager', 3);
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('Henry Anderson', 'Accounting Manager', 3);

-- Insert employees under VP of Engineering
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('Ivy Thomas', 'Software Engineer', 5);
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('Jack Moore', 'Software Engineer', 5);

-- Insert employees under VP of Product
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('Kara Clark', 'Product Manager', 6);
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('Liam White', 'Product Designer', 6);

-- Additional hierarchy levels
-- Insert employees under Finance Manager
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('Mona Lewis', 'Financial Analyst', 7);
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('Nina Walker', 'Budget Analyst', 7);

-- Insert employees under COO
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('Oscar Hall', 'Operations Manager', 4);
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('Paul Allen', 'Logistics Coordinator', 4);

-- Insert employees under Software Engineer (a deeper level)
INSERT INTO Employees (FullName, Title, ManagerEmployeeId) VALUES ('Quinn Wright', 'Junior Engineer', 9);
