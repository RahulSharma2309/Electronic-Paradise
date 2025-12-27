# 🗂️ Product Document Catalog (PM/PO View)

This is a catalog of **product documents**: what they are, why they exist, who owns them, and how often they should be updated.

> **Goal:** Make product planning understandable and executable, while staying aligned with the existing engineering roadmap (`docs/PROJECT_ROADMAP.md`).

---

## ✅ Core Product Documents (Minimum Set)

### 1) Product Vision & Principles
- **File:** `docs/product-documents/PRODUCT_VISION_AND_PRINCIPLES.md`
- **Owner:** PM (sign-off by PO + Tech Lead)
- **Cadence:** Quarterly review; update when strategy changes
- **Purpose:** Clarifies *why* the product exists, what we optimize for, and what we won’t compromise on.
- **Outputs:** North Star metric, success metrics, product principles, strategic guardrails.

### 2) Personas & JTBD (Jobs To Be Done)
- **File:** `docs/product-documents/USER_PERSONAS_AND_JTBD.md`
- **Owner:** PM (supported by PO)
- **Cadence:** Refresh every 6 months or when audience changes
- **Purpose:** Defines *who* we build for and *what jobs* they hire us to do.
- **Outputs:** Primary/secondary personas, JTBD statements, top pains/gains.

### 3) SWOT Analysis
- **File:** `docs/product-documents/SWOT_ANALYSIS.md`
- **Owner:** PM (workshop with PO/Engineering)
- **Cadence:** Quarterly (or prior to major roadmap change)
- **Purpose:** A reality-check on internal strengths/weaknesses and market opportunities/threats.
- **Outputs:** Risk register inputs, strategy adjustments, roadmap tradeoffs.

### 4) Product Roadmap (PM/PO)
- **File:** `docs/product-documents/PRODUCT_ROADMAP_PM_PO.md`
- **Owner:** PM (content), PO (feasibility + sequencing), Tech Lead (technical constraints)
- **Cadence:** Monthly refinement; formal quarterly update
- **Purpose:** Converts vision into time-phased outcomes and releases.
- **Outputs:** Now/Next/Later + quarterly themes + release goals + key bets/risks.
- **References:** `docs/PROJECT_ROADMAP.md` for engineering epics/PBIs.

### 5) Iteration View (How the Product “Looks” Over Time)
- **File:** `docs/product-documents/ITERATION_VIEW_PM_PO.md`
- **Owner:** PO (primary), PM (outcome framing), Engineering (input)
- **Cadence:** Sprint planning + end-of-sprint updates
- **Purpose:** Describes, in plain language, what the product experience becomes each iteration and how success is judged.
- **Outputs:** Sprint goals, demo narratives, expected user-visible changes, PM/PO checklists.

### 6) Definition of Done (DoD)
- **File:** `docs/product-documents/DEFINITION_OF_DONE.md`
- **Owner:** PO (primary), Engineering (quality gates), PM (release readiness)
- **Cadence:** Quarterly review or when quality/process changes
- **Purpose:** Makes “done” unambiguous at multiple horizons (story → sprint → release → quarter → year).
- **Outputs:** DoD checklists for sprint, quarter, half-year, year.

---

## ⭐ Recommended Product Documents (Often Needed)

### 7) Product Requirements Document (PRD) Template (Optional)
- **Status:** Optional in this repo because `docs/PROJECT_ROADMAP.md` already contains detailed PBIs/acceptance criteria.
- **Owner:** PM (problem framing) + PO (acceptance criteria)
- **Cadence:** Per major initiative/epic
- **Purpose:** Standardizes problem statement, scope, constraints, success metrics, and rollout plan.

### 8) Release Notes + Change Log (Product-facing)
- **Owner:** PO (draft), PM (messaging)
- **Cadence:** Every release / at least quarterly
- **Purpose:** Communicates what changed, why it matters, and what’s next.

### 9) KPI/OKR Dashboard Spec
- **Owner:** PM
- **Cadence:** Quarterly OKR cycle
- **Purpose:** Defines the metrics we track and how we interpret success.

---

## 🔗 Existing Engineering Docs (This Repo Already Has These)

These are not “product docs” but are critical references:

- **Engineering roadmap:** `docs/PROJECT_ROADMAP.md`
- **Execution checklist:** `docs/ITERATION_CHECKLIST.md`
- **User journey flows:** `docs/Functionality/`
- **Service architecture:** `docs/Services/`
- **Tech stack decisions:** `docs/TECH_STACK.md`

