# 🛰️ Multi-Service Ping Web API (Availability Fanout Service)

## Overview

This lightweight web service acts as a **centralized health check endpoint** that pings multiple downstream services in a single request. It's designed to optimize **monitoring efficiency, reduce Azure costs**, and **keep low-traffic apps warm**—especially useful for services hosted on free or consumption-based tiers.

Instead of configuring individual availability tests for each downstream service, this API allows you to **consolidate checks**—reducing operational overhead and significantly lowering Azure monitoring costs.

---

## 🚀 Features

- 🔗 Pings multiple downstream services (HTTP/HTTPS endpoints) from a single API call
- 📊 Aggregates response times, status codes, and error messages
- 🔒 Optional authentication for secure access
- 🧠 Simple JSON configuration for service targets
- 📦 Deployable as an Azure App Service, Container App, or Function

---

<img width="1564" height="1470" alt="AvailabilityFanoutSvc-original" src="https://github.com/user-attachments/assets/a60d7628-f00f-468a-838a-8a978484fc60" />

<img width="2152" height="1484" alt="AvailabilityFanoutSvc-updated" src="https://github.com/user-attachments/assets/70d018aa-d9a1-47e6-bcfa-c531f8d19a68" />

---

## 💰 Azure Cost Optimization

### Azure Availability Tests Pricing Model

Azure charges for availability tests based on the **number of tests**, **frequency**, and **test locations**. As of current pricing:

| Metric                     | Cost Impact                            |
|---------------------------|----------------------------------------|
| Number of tests            | Each test incurs a monthly cost        |
| Test frequency             | Higher frequency = higher cost         |
| Number of test locations   | Each location multiplies the cost      |

> Example: 5 services tested every 5 minutes from 3 regions = **15 tests**  
> Consolidated via this API = **1 test** hitting all 5 services

### 1:N vs 1:1 Tradeoff

| Strategy     | Pros                                           | Cons                                           |
|--------------|------------------------------------------------|------------------------------------------------|
| **1:1**      | Granular visibility per service                | High cost, complex configuration               |
| **1:N**      | Cost-efficient, centralized monitoring         | Less granular, single point of aggregation     |

This API enables a **1:N strategy**, where one availability test triggers health checks for multiple services. It's ideal for:

- Microservices architectures
- Internal APIs with low external visibility
- Cost-sensitive environments

---

## 🔥 Warm-Up Benefits

Free-tier and consumption-based hosting plans (like Azure App Service Free or Azure Functions Consumption) often **deallocate idle apps** after ~15 minutes of inactivity. This leads to:

- ⏳ Cold starts with high latency
- 🧊 Delayed availability for users or monitoring tools
- ❌ Failed health checks due to timeouts

By configuring this service to **periodically ping downstream apps**, you can:

- ✅ Keep apps "warm" and responsive
- 🚀 Reduce cold start latency for critical endpoints
- 🔄 Maintain readiness for availability tests and user traffic

> Example: A downstream service hosted on Azure App Service Free will stay active if pinged every 5–10 minutes via this API, avoiding deallocation and ensuring consistent uptime.

---

## 🛠️ Usage

### Request

```http
GET /v1/ping
