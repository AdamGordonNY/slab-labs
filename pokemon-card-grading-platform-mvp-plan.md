# Pokemon Card Grading Platform - 90-Day MVP Development Plan

## 1. Executive summary

This plan delivers a production-ready MVP for a Pokemon card grading platform in 90 days. The MVP focuses on the core commercial workflow: customer registration, submission intake, payment collection, grading operations, customer tracking, and admin oversight.

The platform will support three core roles:
- Customer
- Grader
- Admin

The MVP excludes Tier 3 features unless the core flow is fully stable and the team has time remaining. Those deferred items include CSV bulk import, promo codes, SMS notifications, 2FA for admins, advanced reporting, and optional Redis caching.

## 2. 90-day milestone board

| Milestone | Days | Outcome | Deliverables |
|---|---:|---|---|
| Foundation & setup | 1-15 | Scope locked, architecture approved, environment ready | Repo setup, CI/CD, schema draft, design system, backlog |
| Customer auth & submission flow | 16-35 | Customers can register, verify, log in, and submit cards | Auth flows, image upload, submission form, receipts |
| Payments & lifecycle tracking | 36-55 | Orders are paid and tracked through grading status | Stripe checkout, tax logic, payment webhooks, portal tracking |
| Grading & admin operations | 56-75 | Graders complete work and admins manage process | Grader queue, grade input, cert generation, admin dashboard |
| QA, hardening & launch | 76-90 | Product is tested, secure, monitored, and launch-ready | Security review, monitoring, runbook, launch checklist |

## 3. Task log checklist with substeps

### Sprint 1: Discovery and scaffolding (Days 1-7)

- [ ] Task: Confirm product scope and MVP boundaries
  - [ ] Document MVP features and user journeys
  - [ ] Confirm Tier 3 items are intentionally deferred
  - [ ] Validate role definitions: Customer, Grader, Admin
  - [ ] Align product and engineering on deliverables
  - [ ] Capture acceptance criteria in the backlog

- [ ] Task: Finalize technical architecture
  - [ ] Select frontend, backend, database, storage, and auth stack
  - [ ] Choose payment, email, and monitoring vendors
  - [ ] Define hosting and deployment model
  - [ ] Document core service boundaries and integrations
  - [ ] Review architecture with stakeholders for sign-off

- [ ] Task: Set up repository and development environment
  - [ ] Initialize repo structure and branch strategy
  - [ ] Configure ESLint, Prettier, TypeScript, and tests
  - [ ] Add CI/CD workflow for lint/test/build
  - [ ] Add environment variable template and secrets management
  - [ ] Validate local setup for all devs

- [ ] Task: Create schema draft and API contracts
  - [ ] Define users, submissions, orders, passwords, payments, and grades
  - [ ] Draft inventory, invoices, notifications, and audit tables
  - [ ] Define route list and payload shapes for core APIs
  - [ ] Document validation rules and edge cases
  - [ ] Review schema with engineering before locking it down

- [ ] Task: Create initial design system and wireframes
  - [ ] Design login, signup, dashboard, submission, portal, grading, and admin screens
  - [ ] Define responsive behavior on mobile/tablet/desktop
  - [ ] Validate accessibility basics (labels, contrast, keyboard flow)
  - [ ] Finalize landing page and key user journey screens
  - [ ] Share mockups for approval

### Sprint 2: Authentication and account foundations (Days 8-14)

- [ ] Task: Implement user registration and login
  - [ ] Build signup form with email and password validation
  - [ ] Hash passwords securely before persistence
  - [ ] Reject duplicate emails and invalid inputs
  - [ ] Assign Customer role on first signup
  - [ ] Test success and failure flows

- [ ] Task: Implement email verification
  - [ ] Create verification email template
  - [ ] Generate secure verification token
  - [ ] Add verification endpoint and validation logic
  - [ ] Prevent unverified users from completing protected actions
  - [ ] Test link expiration and invalid token behavior

- [ ] Task: Implement JWT session management
  - [ ] Generate signed JWT on successful login
  - [ ] Store token in HTTP-only secure cookie
  - [ ] Add middleware for route protection
  - [ ] Implement token refresh/logout flow
  - [ ] Verify secure auth behavior in dev and staging

- [ ] Task: Implement password reset flow
  - [ ] Create reset request endpoint and email template
  - [ ] Generate a secure reset token
  - [ ] Add password reset page and form validation
  - [ ] Enforce expiration and single-use token handling
  - [ ] Test invalid reset attempts and expired links

- [ ] Task: Enforce role-based access
  - [ ] Add guard middleware for Customer/Grader/Admin routes
  - [ ] Restrict admin-only actions to admin role only
  - [ ] Ensure graders cannot access customer data or admin tools
  - [ ] Validate route-level access patterns in staging

- [ ] Task: Add rate limiting and security hardening
  - [ ] Add rate limits to login, signup, and reset endpoints
  - [ ] Implement CSRF protection for state-changing requests
  - [ ] Ensure no secrets or PII appear in logs
  - [ ] Validate repeated attack patterns in dev environment

### Sprint 3: Submission intake and receipt flow (Days 15-21)

- [ ] Task: Build submission form
  - [ ] Create form fields for card metadata and order information
  - [ ] Add required-field validation and empty-state handling
  - [ ] Support mobile and desktop layout requirements
  - [ ] Add progress/save behavior for partial entries
  - [ ] Validate all field mappings to backend schema

- [ ] Task: Build image upload and preview
  - [ ] Add file picker for card images
  - [ ] Enforce file type and max-size validation
  - [ ] Display live preview before submission
  - [ ] Upload image to S3-compatible storage
  - [ ] Persist image URL and dimensions in submission record

- [ ] Task: Add service tier selection
  - [ ] Implement Economy, Standard, and Express choices
  - [ ] Display pricing and turnaround estimate before payment
  - [ ] Save selected tier to submission and order data
  - [ ] Validate tier availability and active/inactive state

- [ ] Task: Submit and confirm order
  - [ ] Create submission record with correct status
  - [ ] Create confirmation page with order summary
  - [ ] Trigger order confirmation email to the user
  - [ ] Write audit log entries for submission creation
  - [ ] Test a successful submission end-to-end

### Sprint 4: Payment and order lifecycle (Days 22-30)

- [ ] Task: Implement Stripe checkout
  - [ ] Create Stripe checkout session from user order
  - [ ] Validate amount, currency, and metadata
  - [ ] Redirect user back to success or failure page
  - [ ] Handle failed or abandoned payment cases

- [ ] Task: Implement Stripe webhook handling
  - [ ] Validate Stripe signature and payload integrity
  - [ ] Update order and payment status from webhook events
  - [ ] Handle duplicate webhook events idempotently
  - [ ] Log webhook events and payment processing metadata
  - [ ] Test success and failure webhook coverage

- [ ] Task: Add tax calculation
  - [ ] Determine tax rules based on customer billing location
  - [ ] Calculate subtotal, tax, and total correctly
  - [ ] Store final billing values on order and invoice records
  - [ ] Validate tax logic for different regions

- [ ] Task: Generate and send invoices
  - [ ] Create invoice record after successful payment
  - [ ] Generate PDF or downloadable invoice artifact
  - [ ] Send invoice email to customer
  - [ ] Make invoice data queryable by user and admin

- [ ] Task: Build order tracking page
  - [ ] Create customer-facing order detail view
  - [ ] Show timeline of status changes
  - [ ] Display submission metadata and payment info
  - [ ] Allow user to view shipping status and next steps

### Sprint 5: Customer portal and notifications (Days 31-38)

- [ ] Task: Build customer dashboard
  - [ ] List all submissions and orders for a signed-in customer
  - [ ] Display filters and sorting by status/date
  - [ ] Include links to details and invoice capture
  - [ ] Ensure dashboard loads quickly and cleanly

- [ ] Task: Add certificate download
  - [ ] Allow download after grading is complete
  - [ ] Store certificate asset in protected storage
  - [ ] Restrict access to the owner or admin users
  - [ ] Validate certificate generation and download flow

- [ ] Task: Implement account settings
  - [ ] Build profile update form
  - [ ] Support email change verification flow
  - [ ] Log security-sensitive changes
  - [ ] Validate update behavior and permission rules

- [ ] Task: Add resubmit quick action
  - [ ] Build resubmission from existing order
  - [ ] Reuse card metadata and original context
  - [ ] Record audit trail for the resubmission event
  - [ ] Verify it routes correctly into the submission flow

- [ ] Task: Trigger lifecycle emails
  - [ ] Add submission received email template and logic
  - [ ] Add grading started email trigger
  - [ ] Add grading complete email trigger
  - [ ] Add ready-to-ship email trigger
  - [ ] Monitor send failures and retry logic

### Sprint 6: Grader workflow (Days 39-46)

- [ ] Task: Build grader queue dashboard
  - [ ] Show all submissions awaiting grading
  - [ ] Allow filtering by status, tier, and date
  - [ ] Prioritize queue state transitions correctly
  - [ ] Ensure grader dashboard is authorized and role-protected

- [ ] Task: Implement card reference lookup
  - [ ] Search by set name and card number
  - [ ] Display matching catalog metadata
  - [ ] Match grading item to reference record
  - [ ] Handle no-match and partial-match edge cases

- [ ] Task: Build grade entry form
  - [ ] Add grade input controls with validation
  - [ ] Capture grader notes and condition comments
  - [ ] Save grade to database and update overall status
  - [ ] Validate grading workflow for accepted/rejected values

- [ ] Task: Generate cert numbers
  - [ ] Create unique cert numbering scheme
  - [ ] Save cert number with grade record
  - [ ] Prevent duplicates and handle collisions
  - [ ] Expose cert number to customer and admin views

- [ ] Task: Track inventory status movement
  - [ ] Move submission through queue → grading → complete → shipped
  - [ ] Update inventory table and status history
  - [ ] Log each status transition in audit log
  - [ ] Ensure admin and customer views reflect current state

### Sprint 7: Admin dashboard (Days 47-55)

- [ ] Task: Build admin overview dashboard
  - [ ] Show submissions per day, revenue, turnaround metrics
  - [ ] Pull aggregate values from correct source tables
  - [ ] Present KPIs in readable summary cards or charts
  - [ ] Ensure data refresh works correctly

- [ ] Task: Add user management controls
  - [ ] Allow admins to activate/deactivate graders
  - [ ] Restrict access for inactive users immediately
  - [ ] Record all changes in audit logs
  - [ ] Validate onboarding and offboarding workflows

- [ ] Task: Add manual order controls
  - [ ] Let admin override or advance order states
  - [ ] Add confirmation workflow before action
  - [ ] Log all manual interventions with actor info
  - [ ] Protect manual controls with admin-only authorization

- [ ] Task: Build basic revenue and tier analytics
  - [ ] Summarize revenue by time period and tier
  - [ ] Display active submissions by tier
  - [ ] Validate summary metrics against source records
  - [ ] Ensure charts handle empty data sets gracefully

### Sprint 8: Quality, security, and performance (Days 56-63)

- [ ] Task: Audit validation and sanitization
  - [ ] Review all user-controlled input fields
  - [ ] Add server-side validation to every API route
  - [ ] Guard against malformed or malicious payloads
  - [ ] Confirm no SQL injection or unsafe input patterns remain

- [ ] Task: Hardening for auth and routes
  - [ ] Validate CSRF on all state-changing actions
  - [ ] Rate limit auth and payment-sensitive endpoints
  - [ ] Add safe error handling for unauthorized access
  - [ ] Confirm app returns sanitized error responses

- [ ] Task: Optimize database queries and image delivery
  - [ ] Add missing indexes to key lookup columns
  - [ ] Review slow queries on dashboard and queue pages
  - [ ] Improve image lazy-loading and WebP conversion flow
  - [ ] Validate performance under expected transaction volume

- [ ] Task: Improve UX and accessibility
  - [ ] Confirm keyboard navigation for critical actions
  - [ ] Verify form labels, headings, and semantic structure
  - [ ] Test color contrast and touch-target sizing
  - [ ] Run accessibility QA against major user flows

### Sprint 9: Production readiness and operational foundations (Days 64-72)

- [ ] Task: Set up production monitoring
  - [ ] Configure Sentry for frontend and backend exceptions
  - [ ] Enable structured JSON logging across app services
  - [ ] Add alerting for critical service errors and downtime
  - [ ] Validate alert routing to team communication channel

- [ ] Task: Configure health checks and uptime monitoring
  - [ ] Create /api/health endpoint with dependency checks
  - [ ] Add uptime monitoring for core pages and API routes
  - [ ] Define alert response and escalation path
  - [ ] Validate health check under failure conditions

- [ ] Task: Backup and disaster recovery validation
  - [ ] Configure automated DB backup schedule
  - [ ] Test restore workflow in staging
  - [ ] Document disaster recovery runbook
  - [ ] Share backup and restore steps with operations team

- [ ] Task: Prepare compliance and privacy controls
  - [ ] Define GDPR data export and deletion workflow
  - [ ] Enable PII access logging for protected data
  - [ ] Draft privacy policy, ToS, and retention policy entries
  - [ ] Confirm data retention schedule for completed orders

### Sprint 10: Full QA, launch testing, and stabilization (Days 73-90)

- [ ] Task: Run end-to-end QA across critical flows
  - [ ] Test signup/login and password reset flows
  - [ ] Test submission and payment workflow end-to-end
  - [ ] Test grading and certificate generation flow
  - [ ] Test admin dashboard and user controls
  - [ ] Log regressions and confirm fixes

- [ ] Task: Complete regression and bugfix cycle
  - [ ] Prioritize and fix high-impact defects
  - [ ] Validate fixes in staging before release
  - [ ] Confirm no blocker remains for core workflows
  - [ ] Review rollout risk for all open defects

- [ ] Task: Final production deployment checklist
  - [ ] Validate environment variables and credentials
  - [ ] Confirm DB migration ordering and rollback plan
  - [ ] Verify monitoring and backup systems are active
  - [ ] Review support procedures and incident ownership

- [ ] Task: Soft launch and monitor
  - [ ] Deploy to production or internal-facing controlled rollout
  - [ ] Monitor app health, payments, and submissions for regressions
  - [ ] Triage incidents quickly and document follow-up actions
  - [ ] Confirm launch readiness and stabilize before broader rollout

## 4. Quick task log summary

- [ ] Sprint 1: Discovery and scaffolding
- [ ] Sprint 2: Authentication and account foundations
- [ ] Sprint 3: Submission intake and receipt flow
- [ ] Sprint 4: Payment and order lifecycle
- [ ] Sprint 5: Customer portal and notifications
- [ ] Sprint 6: Grader workflow
- [ ] Sprint 7: Admin dashboard
- [ ] Sprint 8: Quality, security, and performance
- [ ] Sprint 9: Production readiness and operations
- [ ] Sprint 10: Full QA and launch

This checklist-style task log makes each Sprint easier to track and execute as a working implementation board.

## 4. Jira-style epic and story breakdown

### Epic 1: Authentication and account management
- Story 1: As a new customer, I can create an account with email and password
  - Acceptance criteria:
    - User can sign up with valid email and password
    - Password validation is enforced
    - User is assigned Customer role
- Story 2: As a user, I can verify my email address
  - Acceptance criteria:
    - Verification email is sent
    - Link verifies email successfully
    - Unverified users are blocked from critical actions
- Story 3: As a user, I can log in and stay authenticated
  - Acceptance criteria:
    - JWT token is issued securely
    - Session persists via HTTP-only cookie
    - logout clears user session
- Story 4: As a user, I can reset my password
  - Acceptance criteria:
    - Reset email is sent
    - Token expires after configured duration
    - User can set a new password

### Epic 2: Submission management
- Story 5: As a customer, I can submit a card for grading
  - Acceptance criteria:
    - Required metadata is entered
    - Image is uploaded and previewed
    - Submission is stored and assigned a status
- Story 6: As a customer, I can choose a service tier
  - Acceptance criteria:
    - Economy, Standard, and Express are selectable
    - Tier pricing is displayed before payment
    - Selected tier persists in the order
- Story 7: As a customer, I receive confirmation after placing an order
  - Acceptance criteria:
    - Confirmation page displays order summary
    - Receipt email is sent to user
    - Submission is marked as `submitted`

### Epic 3: Billing and payment
- Story 8: As a customer, I can pay for my order
  - Acceptance criteria:
    - Stripe checkout opens successfully
    - Payment success updates order record
    - Payment status is stored and auditable
- Story 9: As a system, I can handle tax and invoice generation
  - Acceptance criteria:
    - Tax is computed based on location
    - Invoice record exists for each paid order
    - Invoice is emailed to customer

### Epic 4: Order tracking and customer portal
- Story 10: As a customer, I can track my order status
  - Acceptance criteria:
    - Current status and history are visible
    - Page includes order details and timeline
- Story 11: As a customer, I can download my grading certificate
  - Acceptance criteria:
    - Certificate is available after grading completion
    - Access is restricted to authorized user
- Story 12: As a customer, I can manage my account settings
  - Acceptance criteria:
    - Profile updates work
    - Email changes are verified properly

### Epic 5: Grading workflow
- Story 13: As a grader, I can see the grading queue
  - Acceptance criteria:
    - Queue displays all items ready to grade
    - Items can be filtered and sorted
- Story 14: As a grader, I can look up card details and record a grade
  - Acceptance criteria:
    - Card set and number search works
    - Grade and notes can be saved
    - Grade results update order state
- Story 15: As a grader, I can generate a cert number
  - Acceptance criteria:
    - Number is unique and persisted
    - Number is displayed to customer and admin

### Epic 6: Admin operations
- Story 16: As an admin, I can view KPIs
  - Acceptance criteria:
    - Dashboard shows submissions, revenue, and turnaround metrics
    - Data reflects current database values
- Story 17: As an admin, I can manage grader access
  - Acceptance criteria:
    - Admin can activate or deactivate user accounts
    - Access is enforced immediately
- Story 18: As an admin, I can intervene on orders manually
  - Acceptance criteria:
    - Admin can update order state
    - Manual actions are logged and auditable

### Epic 7: Notifications and compliance
- Story 19: As a customer, I receive lifecycle notifications
  - Acceptance criteria:
    - Email is sent for order received, grading started, grading complete, ready to ship
- Story 20: As a platform owner, I have operational logging and compliance controls
  - Acceptance criteria:
    - Audit logs capture critical actions
    - GDPR and retention policies are implemented in process

## 5. Architectural plan

### Stack selection
- Frontend: Next.js + TypeScript + Tailwind CSS
- API: Next.js API routes and server actions
- Database: PostgreSQL
- Storage: S3-compatible object storage for images and certificates
- Payment: Stripe Checkout + Webhooks
- Auth: JWT in HTTP-only cookies
- Email: transactional email provider
- Observability: Sentry, structured logging, uptime checks
- Hosting: Vercel for frontend/API and managed Postgres + S3 backend

### System boundaries
- Customer web app
- Grader web app
- Admin web app
- Auth and session layer
- Submission and order processing service
- Payment orchestration layer
- Notification service
- Analytics and dashboard aggregation layer

### Non-functional requirements to satisfy
- Page load under 3s for key pages
- API p95 response under 500ms for standard actions
- Image optimization using lazy loading and WebP conversion
- Database indexes on high-access fields
- HTTPS/TLS and secure cookies
- CSRF and rate limiting
- 99.5% uptime target through managed infrastructure
- Audit logging for all critical actions
- GDPR-friendly data handling and retention policies

## 6. Database schema plan

### Core tables

#### users
- id
- email
- password_hash
- first_name
- last_name
- role
- email_verified_at
- status
- created_at
- updated_at
- last_login_at

#### service_tiers
- id
- code
- name
- price
- turnaround_days
- active
- created_at

#### submissions
- id
- user_id
- service_tier_id
- status
- set_name
- card_number
- card_name
- metadata_json
- image_url
- created_at
- updated_at
- paid_at

#### orders
- id
- user_id
- submission_id
- status
- subtotal
- tax_amount
- total_amount
- currency
- stripe_checkout_session_id
- invoice_number
- created_at
- updated_at
- paid_at

#### payments
- id
- order_id
- user_id
- stripe_payment_id
- amount
- currency
- status
- failure_reason
- created_at
- updated_at

#### grades
- id
- submission_id
- grader_id
- card_reference_id
- grade_value
- grade_notes
- cert_number
- created_at
- updated_at

#### inventory
- id
- submission_id
- current_status
- assigned_grader_id
- location
- updated_at

#### notifications
- id
- user_id
- type
- channel
- subject
- body
- sent_at
- status

#### audit_logs
- id
- user_id
- actor_user_id
- action
- entity_type
- entity_id
- metadata_json
- created_at

### Key indexes
- users.email unique
- submissions.user_id
- submissions.status
- orders.user_id
- orders.status
- grades.submission_id
- grades.cert_number unique
- inventory.submission_id
- audit_logs.entity_type, entity_id

## 7. Definition of done

A feature is done when:
- It passes acceptance criteria
- It is covered by smoke tests and targeted regression checks
- It has server-side validation and access control
- It logs critical changes to audit history
- It is usable on mobile and desktop
- Error states and empty states are handled
- Monitoring and alerts are in place for production-critical behavior

## 8. Recommended delivery team

- 1 Product owner / project lead
- 1 Frontend engineer
- 1 Backend engineer
- 1 QA / test engineer
- 1 Designer / UX resource
- 1 DevOps / platform support (part-time or shared)

## 9. Suggested release strategy

- Staging release at the end of each sprint
- Production soft launch after Sprint 10
- Keep Tier 3 items out of initial release unless team throughput remains above target
- Prioritize stable payment, grading, and order lifecycle before reporting enhancements

## 10. Immediate next action

Proceed with Sprint 1 tasks and lock the MVP scope. The most critical path to protect is:
1. Auth + user roles
2. Submission creation and validation
3. Payment flow with Stripe
4. Order state management and grading queue
5. Admin metrics and monitoring

Once the team confirms these items in Sprint 1, the rest of the plan can proceed without major rework.

## 11. Daily execution checklist (Days 1-90)

### Week 1 (Days 1-7)

- [ ] Day 1: Kickoff and requirements alignment
  - [ ] Review the MVP requirement set and confirm scope
  - [ ] Confirm user roles and major user journeys
  - [ ] Document Tier 3 exclusions and launch assumptions

- [ ] Day 2: Product and engineering alignment
  - [ ] Confirm acceptance criteria for each core flow
  - [ ] Define release goals and success metrics
  - [ ] Assign owners for frontend, backend, QA, and platform

- [ ] Day 3: Architecture finalization
  - [ ] Select Stack: Next.js, TypeScript, Tailwind, PostgreSQL, Stripe, S3
  - [ ] Confirm deployment model and environment strategy
  - [ ] Document integration boundaries for auth, payments, emails, and storage

- [ ] Day 4: Repo and CI setup
  - [ ] Initialize repository structure and branch strategy
  - [ ] Configure linting, formatting, and TypeScript
  - [ ] Add CI workflow for tests, lint, and build checks

- [ ] Day 5: Secrets and local environment setup
  - [ ] Create .env template and secret storage plan
  - [ ] Validate local environment for all developers
  - [ ] Confirm database and storage connection setup

- [ ] Day 6: Schema and API contract draft
  - [ ] Draft tables for users, submissions, orders, grades, payments, and inventory
  - [ ] Define core API routes and payload shapes
  - [ ] Review validation rules and edge cases with engineering

- [ ] Day 7: UX discovery and wireframes
  - [ ] Complete customer sign-up, login, and submission flow mocks
  - [ ] Draft grader queue and admin dashboard layouts
  - [ ] Review mockups and capture approved design baseline

### Week 2 (Days 8-14)

- [ ] Day 8: Auth model implementation setup
  - [ ] Configure user model and DB migration files
  - [ ] Define role enum and permissions model
  - [ ] Prepare auth route structure and middleware layers

- [ ] Day 9: Signup and login flow
  - [ ] Build signup form with validation
  - [ ] Enforce password policy and duplicate-email checks
  - [ ] Connect signup to user creation and default role assignment

- [ ] Day 10: Login and session handling
  - [ ] Implement secure JWT creation on login
  - [ ] Store session token in HTTP-only cookie
  - [ ] Validate login behavior and logout flow

- [ ] Day 11: Email verification
  - [ ] Create verification email template and token generation
  - [ ] Add verification endpoint and validation
  - [ ] Prevent unverified users from protected actions

- [ ] Day 12: Password reset
  - [ ] Build request reset form and endpoint
  - [ ] Generate secure reset token and email flow
  - [ ] Validate reset page and password update logic

- [ ] Day 13: Role-based access control
  - [ ] Add Customer, Grader, and Admin route guards
  - [ ] Restrict admin actions to admin users only
  - [ ] Validate access control for public vs protected routes

- [ ] Day 14: Auth hardening and QA
  - [ ] Add rate limits to auth endpoints
  - [ ] Enforce CSRF protection on state-changing requests
  - [ ] Run smoke test of auth flow across all roles

### Week 3 (Days 15-21)

- [ ] Day 15: Submission form foundation
  - [ ] Build customer submission form shell
  - [ ] Add form validation for required metadata
  - [ ] Confirm layout on mobile and desktop views

- [ ] Day 16: File upload and preview
  - [ ] Implement image selection UI
  - [ ] Validate type and max-size rules
  - [ ] Show live preview before final submission

- [ ] Day 17: Card metadata and tier selection
  - [ ] Add card set, number, and NM details fields
  - [ ] Build tier selector for Economy/Standard/Express
  - [ ] Display price, turnaround, and summary before payment

- [ ] Day 18: Submission API and DB persistence
  - [ ] Create submission endpoint with validation
  - [ ] Save submission and image metadata to DB
  - [ ] Write audit log for submission creation

- [ ] Day 19: Submission confirmation and receipt email
  - [ ] Create confirmation page with order summary
  - [ ] Trigger confirmation email after successful submission
  - [ ] Verify user sees a clean post-submit experience

- [ ] Day 20: Submission QA and bug fix pass
  - [ ] Test form validation failures and success states
  - [ ] Verify image uploads and object storage path
  - [ ] Correct edge cases from QA notes

- [ ] Day 21: Submission milestone demo
  - [ ] Demo working submission flow to stakeholders
  - [ ] Fix feedback items from demo
  - [ ] Close out Sprint 3 tasks and confirm readiness

### Week 4 (Days 22-28)

- [ ] Day 22: Stripe checkout integration
  - [ ] Create order pricing summary for Stripe session
  - [ ] Build checkout session endpoint
  - [ ] Confirm redirect success/failure flow

- [ ] Day 23: Payment webhook implementation
  - [ ] Validate Stripe signature and payload
  - [ ] Update payment and order status from webhook
  - [ ] Handle idempotent webhook retries

- [ ] Day 24: Tax calculation and invoice backend
  - [ ] Add tax calculation service for order totals
  - [ ] Save tax and final total in order records
  - [ ] Generate invoice entry in DB

- [ ] Day 25: Invoice email and user receipt
  - [ ] Generate invoice artifact or PDF reference
  - [ ] Send invoice email to customer
  - [ ] Verify invoice data is accessible to the user

- [ ] Day 26: Order tracking page
  - [ ] Build order detail page for customer portal
  - [ ] Add timeline of order status changes
  - [ ] Show progression from paid to queued to grading

- [ ] Day 27: Payment edge-case testing
  - [ ] Test failed payment, abandoned checkout, and duplicate event cases
  - [ ] Validate invoice and status consistency
  - [ ] Fix payment issues from QA notes

- [ ] Day 28: Payment milestone demo
  - [ ] Demo complete payment-to-order flow
  - [ ] Review outstanding issues and finalize implementation

### Week 5 (Days 29-35)

- [ ] Day 29: Customer dashboard layout
  - [ ] Build order history and recent submissions view
  - [ ] Add filters and sorting by date/status
  - [ ] Ensure dashboard is responsive and role protected

- [ ] Day 30: Dashboard data wiring
  - [ ] Query submitted orders for the current customer
  - [ ] Display correct status badges and summaries
  - [ ] Ensure null/empty states are handled cleanly

- [ ] Day 31: Account settings
  - [ ] Create profile update page
  - [ ] Add email settings and security-sensitive audit logging
  - [ ] Validate update handling and permission model

- [ ] Day 32: Certificate download flow
  - [ ] Store completed grading certificate asset
  - [ ] Restrict access to claim owner or admin
  - [ ] Add download button in customer portal

- [ ] Day 33: Resubmit quick action
  - [ ] Add quick resubmission link in customer account
  - [ ] Reuse original metadata and context
  - [ ] Confirm resubmission record is captured in audit log

- [ ] Day 34: Notification engine setup
  - [ ] Define email types and trigger events
  - [ ] Add templates for order received, grading started, complete, and ship-ready
  - [ ] Validate notification persistence and status tracking

- [ ] Day 35: Sprint review and stabilization
  - [ ] Run portal smoke tests
  - [ ] Fix any issues found in customer-facing flows
  - [ ] Confirm customer portal readiness for grading integration

### Week 6 (Days 36-42)

- [ ] Day 36: Grader access and security
  - [ ] Build grader login/role access
  - [ ] Restrict grader access to grading routes only
  - [ ] Validate admin restrictions and grader session flow

- [ ] Day 37: Grader queue dashboard
  - [ ] Build queue view for grade-ready submissions
  - [ ] Add sorting and status filters
  - [ ] Connect queue to DB queries for pending grading items

- [ ] Day 38: Card lookup and reference data
  - [ ] Build card search by set and number
  - [ ] Validate lookup matching to reference database
  - [ ] Handle no result and partial match scenarios

- [ ] Day 39: Grade-entry workflow
  - [ ] Create grade form and notes input
  - [ ] Add validation for allowed grades and notes length
  - [ ] Save grade and update item state

- [ ] Day 40: Cert number generation
  - [ ] Implement unique certificate numbering
  - [ ] Save cert number to grade record
  - [ ] Expose cert number in customer and admin views

- [ ] Day 41: Inventory and workflow tracking
  - [ ] Add inventory table updates on each status transition
  - [ ] Track queue → grading → complete → shipped states
  - [ ] Write status history records for traceability

- [ ] Day 42: Grader flow QA
  - [ ] Test queue, grade entry, and cert generation end-to-end
  - [ ] Fix grading defects and refine UI flow

### Week 7 (Days 43-49)

- [ ] Day 43: Admin dashboard layout
  - [ ] Create admin overview dashboard shell
  - [ ] Add summary cards for submissions, revenue, and turnaround metrics
  - [ ] Prepare data sources for KPIs and analytics

- [ ] Day 44: KPI query and data aggregation
  - [ ] Calculate submissions per day and revenue trend
  - [ ] Pull turnaround and tier distribution summary
  - [ ] Validate dashboard values against database records

- [ ] Day 45: User management
  - [ ] Create user management page for admins
  - [ ] Add activate/deactivate action for graders
  - [ ] Log role and status changes in audit logs

- [ ] Day 46: Manual order controls
  - [ ] Add admin override and status update controls
  - [ ] Require confirmation for manual interventions
  - [ ] Verify permissions and tracking for admin actions

- [ ] Day 47: Basic analytics view
  - [ ] Build revenue trend and tier breakdown charts
  - [ ] Add empty-state behavior for no-data periods
  - [ ] Validate summary accuracy with sample data

- [ ] Day 48: Admin QA sprint review
  - [ ] Test admin dashboard under sample data
  - [ ] Fix broken styles and inaccurate metrics
  - [ ] Capture remaining admin issues for next round

- [ ] Day 49: Admin milestone demo
  - [ ] Demo admin dashboard and grader tools to stakeholders
  - [ ] Resolve feedback and finalize admin scope

### Week 8 (Days 50-56)

- [ ] Day 50: Input validation audit
  - [ ] Review all customer, admin, and grader inputs
  - [ ] Add missing server-side validation and sanitization
  - [ ] Confirm malformed requests are rejected

- [ ] Day 51: Auth and route hardening
  - [ ] Review login, reset, and checkout endpoints for abuse protections
  - [ ] Enforce CSRF on all state-changing actions
  - [ ] Confirm unauthorized routes fail safely

- [ ] Day 52: Database and query optimization
  - [ ] Add missing indexes on user_id, submission_id, and status fields
  - [ ] Review slow dashboard and queue queries
  - [ ] Optimize any query patterns causing lag

- [ ] Day 53: Image optimization and front-end performance
  - [ ] Implement lazy loading and WebP handling
  - [ ] Review key page speed metrics with Lighthouse
  - [ ] Fix slow rendering or oversized asset issues

- [ ] Day 54: Accessibility pass
  - [ ] Check form labels, headings, keyboard navigation, and focus state
  - [ ] Validate touch-friendly controls and contrast
  - [ ] Fix accessibility defects from QA review

- [ ] Day 55: Security review and issue log
  - [ ] Review auth, payment, and file upload endpoints for exposure
  - [ ] Capture remaining high-risk issues for remediation
  - [ ] Prioritize fixes before production launch

- [ ] Day 56: Hardening milestone review
  - [ ] Run security and performance smoke tests
  - [ ] Verify all Sprint 8 tasks are complete or scheduled

### Week 9 (Days 57-63)

- [ ] Day 57: Sentry setup and application logging
  - [ ] Configure error tracking for frontend and backend
  - [ ] Add JSON logging to key services and actions
  - [ ] Confirm logs capture enough context for debugging

- [ ] Day 58: Health check and uptime monitoring
  - [ ] Create /api/health endpoint and service checks
  - [ ] Set up uptime monitoring for production endpoints
  - [ ] Route critical alerts to the team communication channel

- [ ] Day 59: Backup validation and restore testing
  - [ ] Configure automated DB backups
  - [ ] Validate backup creation in staging or test environment
  - [ ] Practice restore flow and document it

- [ ] Day 60: GDPR and retention controls
  - [ ] Define customer data export and deletion process
  - [ ] Configure PII access logs for protected records
  - [ ] Document retention schedule for completed orders

- [ ] Day 61: Runbook and support process
  - [ ] Draft operational runbook for outages and incidents
  - [ ] Define support owner and escalation path
  - [ ] Review support process with engineering and product

- [ ] Day 62: Documentation review
  - [ ] Confirm privacy policy, ToS, and internal runbook coverage
  - [ ] Validate operational checklists for launch readiness
  - [ ] Resolve missing documentation items

- [ ] Day 63: Ops readiness check
  - [ ] Verify monitoring, backups, and support readiness
  - [ ] Identify any remaining launch blocker

### Week 10 (Days 64-70)

- [ ] Day 64: Full end-to-end regression test prep
  - [ ] Prepare staging environment and test accounts
  - [ ] Define end-to-end test cases for critical flows
  - [ ] Confirm all required data fixtures exist

- [ ] Day 65: Customer journey QA
  - [ ] Test signup → verify → login → submit → pay → track order
  - [ ] Validate email confirmations and invoice generation
  - [ ] Capture defects and require fixes before launch

- [ ] Day 66: Grader workflow QA
  - [ ] Test queue → lookup → grade → cert generation → status progression
  - [ ] Review grading data accuracy and user experience
  - [ ] Document edge cases and bug fixes

- [ ] Day 67: Admin workflow QA
  - [ ] Validate overview dashboard, user controls, and manual order actions
  - [ ] Verify revenue and tier analytics accuracy
  - [ ] Fix issues surfaced by admin testing

- [ ] Day 68: Regression bugfix pass
  - [ ] Resolve high-priority defects found in staging
  - [ ] Re-run critical flows after each fix
  - [ ] Ensure no blocking issue remains open

- [ ] Day 69: Production deployment rehearsal
  - [ ] Validate migration plan and environment variables
  - [ ] Review backup and restore steps before cutover
  - [ ] Confirm support and on-call ownership

- [ ] Day 70: Final readiness review
  - [ ] Review launch checklist and open issues
  - [ ] Sign off on production launch readiness

### Week 11 (Days 71-77)

- [ ] Day 71: Production soft launch prep
  - [ ] Finalize production environment check
  - [ ] Validate Stripe live keys, DB access, and storage config
  - [ ] Confirm all monitoring and alerting are live

- [ ] Day 72: Production deployment
  - [ ] Deploy app and database changes to production
  - [ ] Validate health checks immediately after deployment
  - [ ] Watch logs, payments, and app performance closely

- [ ] Day 73: Post-deploy validation
  - [ ] Verify login, submission, ordering, and payment flows work live
  - [ ] Check email notifications and invoice generation
  - [ ] Validate health checks and alert systems

- [ ] Day 74: Soft launch monitoring
  - [ ] Monitor traffic, conversion, and critical app errors
  - [ ] Verify system remains stable under real usage
  - [ ] Capture and triage any production defects

- [ ] Day 75: User feedback and bug triage
  - [ ] Review test and live user issues
  - [ ] Prioritize production fixes by customer impact
  - [ ] Confirm support workflow is functioning

- [ ] Day 76: Stabilization sprint
  - [ ] Fix production issues with highest user impact
  - [ ] Validate each fix with targeted smoke tests
  - [ ] Update runbook if incidents require operational changes

- [ ] Day 77: Launch confidence review
  - [ ] Evaluate production health and stability metrics
  - [ ] Confirm the MVP is ready for continued rollout or broader adoption

### Week 12 (Days 78-84)

- [ ] Day 78: Customer support script review
  - [ ] Confirm support process for order, payment, and grading issues
  - [ ] Document common support cases and resolutions
  - [ ] Update knowledge base or internal support notes

- [ ] Day 79: Performance and reliability check
  - [ ] Inspect page speed, response times, and 500-level errors
  - [ ] Tune queries or image handling if needed
  - [ ] Review server resource usage and scaling state

- [ ] Day 80: Backlog cleanup and Tier 3 review
  - [ ] Revisit deferred Tier 3 items and estimate remaining value
  - [ ] Confirm whether any should be moved into a post-MVP phase
  - [ ] Close or defer backlog items not needed for launch

- [ ] Day 81: User analytics and metrics review
  - [ ] Review conversion, payment completion, and completion rates
  - [ ] Check patterns in user drop-off or failed submissions
  - [ ] Identify product improvements for the next release cycle

- [ ] Day 82: Security and compliance check
  - [ ] Review access logs, audit trails, and retention process
  - [ ] Confirm GDPR and PII handling remains compliant
  - [ ] Resolve any remaining compliance issues

- [ ] Day 83: Final QA sweep
  - [ ] Re-run priority user journeys across front-end and backend
  - [ ] Validate no critical issue remains unresolved
  - [ ] Confirm final fixes are fully deployed

- [ ] Day 84: Launch readiness sign-off
  - [ ] Review all open issues and risk assessment
  - [ ] Approve MVP launch status and operational readiness

### Week 13 (Days 85-90)

- [ ] Day 85: Final milestone reporting
  - [ ] Summarize sprint outcomes and key metrics
  - [ ] Document scope delivered vs deferred
  - [ ] Confirm what is ready for handoff to next phase

- [ ] Day 86: Post-launch documentation review
  - [ ] Verify runbooks, deployment notes, and support docs are up-to-date
  - [ ] Confirm knowledge is retained across the team
  - [ ] Capture major decisions for future reference

- [ ] Day 87: MVP closeout checklist
  - [ ] Confirm all required features are delivered and stable
  - [ ] Validate launch metrics meet project goals
  - [ ] Record remaining enhancements for future roadmap

- [ ] Day 88: Stakeholder review briefing
  - [ ] Present product status and launch outcome
  - [ ] Share known issues and planned next-phase improvements
  - [ ] Align on roadmap beyond the 90-day MVP

- [ ] Day 89: Retro and lessons learned
  - [ ] Review what worked, what slipped, and what needs adjustment
  - [ ] Capture engineering process improvements
  - [ ] Prepare backlog for next phase

- [ ] Day 90: Project close and handoff
  - [ ] Archive final plan, runbook, and backlog state
  - [ ] Confirm all work is done or formally delegated
  - [ ] Close the 90-day MVP development cycle

## 12. Daily task log usage

Use this as the working execution checklist during the 90-day project:
- Check off tasks as each day is completed
- Move delayed tasks into the next suitable day without changing milestone scope
- If a task is blocked, record the blocker in the daily standup log and assign a recovery owner
- Keep the daily checklist aligned with the sprint-level checklist above to avoid drift from the planned MVP.

This version is optimized for running the project as a day-to-day delivery checklist while keeping the sprint-level plan intact.

## 13. Bare minimum checklist for a single person

This is the minimum viable operating checklist for one person building the MVP without dropping critical work. It preserves the important details but removes unnecessary overhead.

### Working rhythm for a solo developer

- [ ] Every day: spend 30 minutes on planning and task selection
- [ ] Every day: spend 3-4 focused hours on the highest-risk item in the current sprint
- [ ] Every day: run lint, typecheck, and relevant tests before stopping
- [ ] Every day: document progress, blockers, and next-step actions
- [ ] Every week: do one end-to-end smoke test of the current critical path
- [ ] Every week: record exact blockers and move unfinished work to the next sprint block

### Non-negotiable sequence

The order below is critical. Do not skip ahead to later features before the earlier pieces are working.

- [ ] Phase 1: Scope lock and architecture
  - [ ] Confirm MVP scope and remove Tier 3 items from the active plan
  - [ ] Define the three roles: Customer, Grader, Admin
  - [ ] Choose stack and hosting strategy
  - [ ] Set up repo, CI, and local dev environment
  - [ ] Draft schema and core API routes
  - [ ] Create the first set of screens for auth and submission

- [ ] Phase 2: Authentication and account system
  - [ ] Build signup, login, logout, and password reset
  - [ ] Add email verification flow
  - [ ] Implement secure JWT sessions using HTTP-only cookies
  - [ ] Add role-based route protections and admin-only access
  - [ ] Add rate limiting and CSRF protection
  - [ ] Validate auth flow end-to-end before continuing

- [ ] Phase 3: Submission intake and image handling
  - [ ] Build customer submission form with metadata
  - [ ] Add image upload, file validation, and preview
  - [ ] Save the submission and image metadata in the database
  - [ ] Connect service tier selection to pricing and turnaround time
  - [ ] Add confirmation page and receipt email
  - [ ] Validate the submission flow works without manual intervention

- [ ] Phase 4: Payment and order lifecycle
  - [ ] Integrate Stripe checkout
  - [ ] Handle payment webhooks and payment status updates
  - [ ] Apply tax and calculate total order cost
  - [ ] Generate invoice record and invoice email
  - [ ] Build customer order status and tracking page
  - [ ] Validate completed payment flow with success and failure cases

- [ ] Phase 5: Customer portal and notifications
  - [ ] Build customer dashboard with submission and order history
  - [ ] Add certificate download after grading completion
  - [ ] Add account settings and profile management
  - [ ] Add quick resubmit flow
  - [ ] Send lifecycle emails for order received, grading started, grading complete, and ready to ship
  - [ ] Validate portal UI and email behavior

- [ ] Phase 6: Grader workflow
  - [ ] Build grader queue view
  - [ ] Add card lookup by set and number
  - [ ] Add grade entry and notes
  - [ ] Generate unique certificate number
  - [ ] Update inventory and status progression
  - [ ] Validate full grading flow from queue to certificate

- [ ] Phase 7: Admin tools
  - [ ] Build overview dashboard with KPIs and basic charts
  - [ ] Add user management for graders
  - [ ] Add manual order controls for admin overrides
  - [ ] Validate admin-only access and audit logging

- [ ] Phase 8: Hardening and quality
  - [ ] Audit validation and sanitization across all endpoints
  - [ ] Review auth, file upload, and checkout security
  - [ ] Optimize database queries and image loading
  - [ ] Fix accessibility issues for keyboard and screen-reader support
  - [ ] Run regression checks for all critical user journeys

- [ ] Phase 9: Operations and launch readiness
  - [ ] Set up error logging and uptime monitoring
  - [ ] Configure /api/health and alerting
  - [ ] Configure backups and disaster recovery checks
  - [ ] Validate GDPR/privacy and retention controls
  - [ ] Prepare runbook and support process

- [ ] Phase 10: Launch
  - [ ] Run complete end-to-end QA in staging
  - [ ] Fix all high-priority defects
  - [ ] Prepare environment variables and deployment checklist
  - [ ] Deploy to production in a controlled rollout
  - [ ] Monitor health, payments, and core flows immediately after launch

### Minimal weekly checklist for solo execution

- [ ] Week 1: Scope, environment, architecture, repo setup, schema draft
- [ ] Week 2: Auth system and security basics
- [ ] Week 3: Submission form, file upload, confirmation flow
- [ ] Week 4: Stripe payment and order lifecycle
- [ ] Week 5: Customer portal and lifecycle emails
- [ ] Week 6: Grader queue and grading workflow
- [ ] Week 7: Admin dashboard and user controls
- [ ] Week 8: Security hardening, performance, UX polish
- [ ] Week 9: Monitoring, backups, compliance, runbook
- [ ] Week 10: QA, final launch prep, deployment, monitoring

### Minimum “done” checks before moving on

- [ ] A route or feature has a working happy path and one failure path
- [ ] Validation and authorization are enforced server-side
- [ ] Audit log captures key state changes
- [ ] No secrets or sensitive data are exposed in logs or responses
- [ ] The feature is tested in a real browser or API smoke test
- [ ] The feature is not left half-finished in a broken state

### Minimum “must not skip” items

- [ ] Do not build the admin dashboard before the auth and order workflow work
- [ ] Do not build advanced reporting before the core platform is stable
- [ ] Do not launch without Stripe, auth, and payment validation
- [ ] Do not skip monitoring, backups, and health checks before production
- [ ] Do not defer all QA until the end; do weekly smoke tests for the current critical path

### Single-person daily template

- [ ] What is the single most important thing to finish today?
- [ ] What is the exact smallest working output for today?
- [ ] What tests or smoke checks will verify the work?
- [ ] What is blocked or needs follow-up?
- [ ] What will be done tomorrow?

This is the leanest checklist that still preserves the required product, security, operational, and launch details for a solo developer.
