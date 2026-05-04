# 🚀 Credit Risk Engine

A configurable, enterprise-grade REST API for customer credit risk classification, personalized credit limit calculation, and monthly income estimation.

Designed as a modular monolith using Clean Architecture principles, this solution emphasizes:

* **Data-driven business rules**
* **Maintainability and extensibility**
* **Deterministic policy enforcement**
* **Comprehensive automated testing**
* **Production-minded containerization and CI/CD**

---

# 📌 Overview

This application classifies customers into predefined credit clusters based on:

* Credit score
* Age
* Market debt profile
* Job title

It then:

* Assigns a job category
* Calculates estimated monthly income
* Computes approved credit limit
* Applies penalty rules
* Enforces cluster caps

All classification and financial rules are externally configurable through JSON configuration rather than hardcoded logic.

---

# 🧠 Key Features

## Core Capabilities

* Customer risk cluster classification
* Job title category prioritization
* Monthly income estimation
* Personalized credit limit calculation
* Penalty enforcement for default debt
* Cluster cap enforcement
* Stateless request processing

## Engineering Features

* Clean Architecture
* Dependency Injection
* Rule Provider abstraction
* Configurable JSON rule engine
* Swagger/OpenAPI documentation
* FluentValidation request validation
* Unit testing
* Integration testing
* Dockerized deployment
* GitHub Actions CI pipeline
* Non-root container runtime
* Global exception handling middleware

---

# 🏗️ Architecture

```txt
credit-risk-engine/
├── src/
│   ├── CreditRiskEngine.Api/
│   ├── CreditRiskEngine.Application/
│   ├── CreditRiskEngine.Domain/
│   └── CreditRiskEngine.Infrastructure/
│
├── tests/
│   ├── CreditRiskEngine.UnitTests/
│   └── CreditRiskEngine.IntegrationTests/
│
├── ai-journey/
├── Dockerfile
├── docker-compose.yml
└── .github/workflows/ci.yml
```

## Layer Responsibilities

### Domain

Contains:

* Entities
* Enums
* Rules
* Core business models
* Rule contracts

### Application

Contains:

* Classification orchestration
* Use cases
* Service interfaces

### Infrastructure

Contains:

* JSON rule loading
* Dependency injection wiring
* External configuration providers

### API

Contains:

* REST controllers
* Request/response contracts
* Validation
* Swagger

---

# ⚙️ Design Decisions

## Why modular monolith?

A modular monolith was intentionally chosen to:

* Avoid unnecessary distributed complexity
* Enforce architectural boundaries
* Maximize maintainability
* Improve testability
* Keep operational overhead low

---

## Why configuration-driven rules?

Business policies are expected to evolve.

Externalizing rules into configuration:

* Decouples policy from deployment
* Improves governance
* Reduces code churn
* Enables future operational flexibility

---

## Why non-root Docker runtime?

Running containers as non-root:

* Reduces security risk
* Aligns with production best practices
* Demonstrates security awareness

---

# 📊 Rule Engine

Rules are loaded from:

```txt
src/CreditRiskEngine.Api/rules.json
```

Configured categories:

* Clusters
* Job categories
* Penalties

This allows policy updates without core logic rewrites.

---

# 🧪 Testing Strategy

## Unit Tests

Coverage includes:

* Cluster assignment boundaries
* Job title keyword matching
* Case-insensitive matching
* Priority ordering
* Credit formula correctness
* Penalty rules
* Cap enforcement
* Monthly income matrix
* CLUSTER_D denial logic

---

## Integration Tests

Coverage includes:

* Full POST `/customers/classify` request/response cycle
* Invalid score handling
* Missing required fields
* HTTP 400 validation responses

---

## Run Tests

```bash
dotnet test
```

---

# 🐳 Docker

## Build

```bash
docker build -t credit-risk-engine .
```

## Run

```bash
docker compose up --build
```

## Swagger

```txt
http://localhost:8080/swagger
```
Interactive API documentation is automatically available via Swagger UI when the application is running.

---

# 🔄 CI/CD

GitHub Actions pipeline includes:

* Restore
* Build
* Test
* Docker build validation

Workflow file:

```txt
.github/workflows/ci.yml
```

---

# 🤖 AI Journey

This repository includes:

```txt
ai-journey/
├── README.md
├── prompts.md
└── learnings.md
```

Documenting:

* AI usage strategy
* Prompt iteration
* Validation of AI outputs
* Design refinement
* Lessons learned

---

# 🔮 Future Improvements

Potential next steps:

* Rule versioning
* Admin configuration UI
* Database-backed policy management
* Audit trails
* Observability dashboards
* Feature flags for policy rollout
* Multi-region policy segmentation

---

# 🛡️ Security Considerations

* Stateless architecture
* Input validation
* Controlled configuration loading
* Non-root containerization
* Deterministic rule evaluation
* Global exception handling to avoid leaking internal stack traces in API responses

---

# 📍 Final Notes

This project was intentionally designed to balance:

* Enterprise readiness
* Architectural clarity
* Practical simplicity
* Scalability
* Maintainability

It is not merely a coding exercise implementation, but rather a production-minded credit policy engine prototype.

---

# 👤 Author

Developed by Tharik Antunes as part of a senior/staff-level backend engineering challenge.
