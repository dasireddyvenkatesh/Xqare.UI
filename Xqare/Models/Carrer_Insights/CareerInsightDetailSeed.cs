using Xqare.Models.Carrer_Insights_Detail;

namespace Xqare.Models.Carrer_Insights
{
    public class CareerInsightDetailSeed
    {
        public static CareerInsightDetailModel Build(string articleId)
        {
            var page = CareerInsightSeed.Build();
            var article = page.Articles.FirstOrDefault(item => item.Id == articleId)
                ?? page.FeaturedArticle
                ?? page.Articles.First();

            var sections = article.Id == "faang-offer"
                ? BuildFaangSections()
                : BuildArticleSections(article);

            return new CareerInsightDetailModel
            {
                Article = article,
                HeroImageUrl = article.ImageUrl,
                KeyTakeaways = BuildTakeaways(article),
                Sections = sections,
                ContinueReading = page.Articles
                    .Where(item => item.Id != article.Id)
                    .Take(3)
                    .ToArray()
            };
        }

        private static IReadOnlyList<string> BuildTakeaways(CareerInsightArticle article) =>
            article.Id switch
            {
                "faang-offer" =>
                [
                    "FAANG hiring is a game with rules: learn the rules and your odds triple",
                "Referrals still matter: a good one can 4x your callback rate",
                "The bar is not genius: it is consistent under pressure across 5 rounds",
                "Behavioral rounds are eliminatory at every level: do not underestimate them",
                "XQARE members with FAANG mentors land offers at meaningfully higher rates"
                ],
                "genai-2026" =>
                [
                    "Hiring teams reward people who can ship GenAI workflows, not just discuss tools",
                "RAG quality depends on retrieval design, evaluation, and production feedback loops",
                "Prompting is useful, but agents, evals, and integration skills separate stronger candidates",
                "A portfolio with measurable AI outcomes is stronger than a certificate-only profile",
                "Mentor review helps you turn experiments into interview-ready proof"
                ],
                "linkedin-guide" =>
                [
                    "Recruiters scan headline, recent role, keywords, and proof of impact first",
                "A specific profile beats a broad one because it signals level and direction",
                "Featured work and quantified bullets make your profile easier to trust",
                "Search-friendly wording helps you appear in the right recruiter filters",
                "A clean outreach system turns profile views into conversations"
                ],
                "mentorship-growth" =>
                [
                    "The best mentors compress years of trial and error into practical feedback",
                "Clear goals make mentorship sessions sharper and more useful",
                "A mentor can help you see blind spots in role choice, portfolio, and interview readiness",
                "Progress comes from repeating small loops: prepare, act, review, improve",
                "The right network creates opportunities before public job posts do"
                ],
                "cloud-roadmap" =>
                [
                    "Cloud roles reward fundamentals before vendor-specific memorization",
                "Hands-on projects are the fastest way to prove readiness",
                "Networking, Linux, security, and automation matter across every cloud path",
                "Certifications help most when paired with deployable work",
                "A mentor can keep the roadmap practical and role-specific"
                ],
                "technical-interviews" =>
                [
                    "Interview performance improves fastest through timed practice and review",
                "Communication is graded alongside correctness",
                "Testing your solution is a signal of senior engineering habits",
                "System design prep should focus on trade-offs, not memorized diagrams",
                "Mock interviews expose gaps before real interviews do"
                ],
                "hiring-skills" =>
                [
                    "Companies are hiring for adaptable builders who can connect product and engineering outcomes",
                "AI, cloud, data, security, and communication are recurring signals",
                "Depth in one stack plus breadth across modern tooling is a strong combination",
                "Proof matters: projects, metrics, and decisions beat buzzwords",
                "Skill plans should be tied to the roles you actually want"
                ],
                "resume-tips" =>
                [
                    "Strong resumes show scope, impact, and technical judgment quickly",
                "Quantified bullets are easier for recruiters and hiring managers to trust",
                "ATS-friendly structure helps, but clarity still wins with humans",
                "Projects should demonstrate the job you want next",
                "Resume review is most useful when it targets a specific role level"
                ],
                "data-engineering" =>
                [
                    "Data engineering is built on SQL, modeling, pipelines, reliability, and cloud systems",
                "Portfolio projects should show ingestion, transformation, quality checks, and serving",
                "Analytics experience can transfer well when paired with production habits",
                "Batch and streaming concepts both matter for modern roles",
                "Mentorship helps you choose the shortest credible transition path"
                ],
                _ =>
                [
                    "Career growth is easier when goals, proof, and feedback stay connected",
                "The strongest candidates show both technical ability and practical judgment",
                "Consistent review cycles create faster improvement than scattered effort",
                "Your portfolio should make your next role obvious",
                "Mentorship helps you avoid slow, avoidable mistakes"
                ]
            };

        private static IReadOnlyList<CareerInsightSection> BuildArticleSections(CareerInsightArticle article) =>
        [
            Section("overview", $"Why {article.Title} Matters",
            Paragraph(article.Summary),
            Paragraph($"For {article.CategoryName.ToLowerInvariant()}, the strongest signal is not that you know the topic. It is that you can explain choices, show proof, and connect your work to business or team outcomes.")),
        Section("positioning", "Positioning Your Profile",
            Bullet("Make the target role obvious", "Use your headline, summary, and recent work to point at one clear direction.", "Mirror the language used in job descriptions without stuffing keywords.", "Show the level you are aiming for through scope, ownership, and measurable outcomes."),
            Bullet("Build proof recruiters can scan", "Add 2-3 concrete projects or achievements.", "Write short notes on decisions, trade-offs, and results.", "Keep links, screenshots, and repositories clean enough to evaluate quickly.")),
        Section("execution", "Practical Action Plan",
            Check("Choose one target role and collect 8-10 representative job descriptions.", "List the repeated skills, tools, and outcomes those roles ask for.", "Update your resume and profile with matching evidence.", "Create one focused project or case study that proves the missing signal.", "Ask a mentor or experienced peer to review the story before applying.")),
        Section("mistakes", "Common Mistakes To Avoid",
            Check("Trying to look relevant for every role at once.", "Listing tools without explaining impact.", "Using generic summaries that do not match the target role.", "Sending applications before your proof points are easy to understand.", "Ignoring feedback after the first few rejections.")),
        Section("mentor-support", "How XQARE Mentors Help",
            Paragraph("XQARE mentors help turn broad career advice into a role-specific plan. They can review your profile, calibrate your level, identify missing proof, and help you practice the conversations that decide whether opportunities move forward."),
            Quote($"A strong {article.CategoryName.ToLowerInvariant()} plan should make the next step feel specific: what to learn, what to build, what to show, and who to talk to."))
        ];

        private static IReadOnlyList<CareerInsightSection> BuildFaangSections() =>
        [
            Section("truth", "The Truth About FAANG Hiring",
            Paragraph("FAANG (or MAANG, or whatever the acronym is this year) hiring isn't a mystery. It's a repeatable process with well-understood mechanics. Candidates who treat it as a game with rules dramatically outperform candidates who treat it as a mystery to solve."),
            Paragraph("This playbook is written by people who've been on both sides of the interview table at Meta, Amazon, Google, Microsoft, and Apple — many of whom are XQARE mentors..")),
            Section("positioning", "Phase 1: Positioning Yourself (Weeks -12 to -8)",
            Paragraph("Before you apply, make yourself a candidate they want."),
            Bullet("Fix Your Resume for FAANG Standards", "One page. Two if you have 8+ years.", "Every bullet quantified: reduced p99 latency by 43% by migrating to gRPC across 4 services handling 12M daily requests.", "No jargon soup. Clarity signals seniority.", "Match your role level to your resume tone."),
            Bullet("Build a Signal-Rich GitHub / Portfolio", "2-3 substantive projects, not 20 forks.", "Clean READMEs with architecture diagrams.", "One well-written blog post about a technical decision you made."),
            Bullet("Get Referrals", "Reach out to XQARE mentors and Circle members who work there.", "Warm, specific asks: I am targeting an L4 SWE role. Would you refer me?", "Send them your resume and a 3-line pitch they can use.")),
        Section("recruiter", "Phase 2: Recruiter Screen (Week 0)",
            Paragraph("The recruiter's job is to gate-keep. Yours is to signal seriousness and level."),
            Check("Answer level-fit questions with specifics.", "Do not disclose current compensation.", "State a specific target range based on levels.fyi and mentor input.", "Ask about the timeline, panel composition, and specific team.")),
        Section("technical-screen", "Phase 3: The Phone/Video Technical Screen",
            Paragraph("Usually 45-60 minutes, one medium DSA problem or two easy-mediums."),
            Check("Communicate before coding: restate the problem, discuss edge cases.", "Start with brute force, then optimize.", "Talk through trade-offs as you code.", "Test your solution manually at the end.", "Ask a great question to close.")),
        Section("onsite", "Phase 4: The Onsite / Virtual Onsite",
            Paragraph("Typically 4-5 rounds."),
            Bullet("Round 1-2: Coding (DSA)", "Medium-hard problems.", "Focus: correctness, communication, complexity analysis, testing.", "Prep: 100-150 problems across patterns; NeetCode 150 is the gold list."),
            Bullet("Round 3: System Design (mid+ candidates only)", "Focus: framework, trade-offs, depth on 1-2 components, scalability.", "Prep: 12-15 classic designs plus strong primitives."),
            Bullet("Round 4: Behavioral / Leadership", "Amazon: strict on Leadership Principles.", "Google: probes for Googliness, ambiguity, collaboration.", "Meta: cross-functional depth, impact stories.", "Apple: deep craft, quiet confidence, opinionated but not dogmatic."),
            Bullet("Round 5: Domain / Bar Raiser", "A senior engineer or Bar Raiser probes depth.", "Focus: judgment, seniority signal, growth mindset.")),
        Section("signals", "The 4 Signals They're Grading",
            Paragraph("Every FAANG interview is scored on some combination of:"),
            Signals(("Problem-Solving", "how you approach unfamiliar problems"), ("Coding / Design", "quality of your solution"), ("Communication", "clarity, structure, collaboration"), ("Culture / Values", "fit for the specific company")),
            Paragraph("Weak in one? You can still pass. Weak in two? You will fail.")),
        Section("mistakes", "Common Mistakes That Kill Otherwise Strong Candidates",
            Check("Diving into code before clarifying signals junior thinking.", "Not testing your solution is a huge red flag.", "Underprepping behavioral is assumed to be easy, actually rigorous.", "Sharing offers you do not have means recruiters check, always.", "Poor negotiation can leave 15-40% to the offer.")),
        Section("framework", "The FAANG Prep Framework: 12 Weeks",
            Check("Weeks 1-2: Baseline assessment, DSA gap analysis, story bank draft.", "Weeks 3-6: Deliberate DSA plus 1 system design/week plus 1 mock/week.", "Weeks 7-9: Company-specific prep, targeted problems, behavioral polish.", "Weeks 10-11: Full mock loops, 2-3 per week.", "Week 12: Rest, review, and interview.")),
        Section("outcomes", "How XQARE FAANG Mentors Change Outcomes",
            Paragraph("XQARE has active mentors at all major FAANG companies. They provide:"),
            Check("Level calibration: are you actually ready for L4, or should you target L3?", "Company-specific prep: Amazon-style interviews are meaningfully different from Google-style.", "Mock interviews that mirror the exact format, cadence, and depth of the real thing.", "Behavioral coaching that flips outcomes for many candidates.", "Referrals from mentors who know you personally.", "Negotiation coaching that often adds meaningful value to the final offer."),
            Quote("I did 6 mock interviews with my XQARE mentor before my Google onsite. In three of them, he asked variations of what I got in the real interviews. Offer came back at L5 first try. — XQARE Member"))
        ];

        private static CareerInsightSection Section(string id, string title, params CareerInsightContentBlock[] blocks) =>
            new() { Id = id, Title = title, Blocks = blocks };

        private static CareerInsightContentBlock Paragraph(string text) =>
            new() { Type = CareerInsightBlockType.Paragraph, Text = text };

        private static CareerInsightContentBlock Quote(string text) =>
            new() { Type = CareerInsightBlockType.Quote, Text = text };

        private static CareerInsightContentBlock Check(params string[] items) =>
            new() { Type = CareerInsightBlockType.CheckList, Items = items };

        private static CareerInsightContentBlock Bullet(string heading, params string[] items) =>
            new() { Type = CareerInsightBlockType.BulletList, Heading = heading, Items = items };

        private static CareerInsightContentBlock Signals(params (string Label, string Description)[] signals) =>
            new()
            {
                Type = CareerInsightBlockType.NumberedSignals,
                Signals = signals.Select(signal => new CareerInsightSignal
                {
                    Label = signal.Label,
                    Description = signal.Description
                }).ToArray()
            };
    }
}
