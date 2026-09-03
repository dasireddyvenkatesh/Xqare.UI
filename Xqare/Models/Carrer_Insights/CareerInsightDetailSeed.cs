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

            var sections = article.Id switch
            {
                "faang-offer" => BuildFaangSections(),
                "linkedin-guide" => BuildLinkedInSections(),
                "mentorship-growth" => BuildMentorshipSections(),
                "cloud-roadmap" => BuildCloudSections(),
                "technical-interviews" => BuildTechnicalInterviewSections(),
                "hiring-skills" => BuildHiringSkillsSections(),


                _ => BuildArticleSections(article)
            };

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

        private static IReadOnlyList<CareerInsightSection> BuildArticleSections(
            CareerInsightArticle article) =>
        [
            Section(
                "overview",
                $"Why {article.Title} Matters",
                Paragraph(article.Summary),
                Paragraph(
                    $"For {article.CategoryName.ToLowerInvariant()}, the strongest signal is not that you know the topic. It is that you can explain choices, show proof, and connect your work to business or team outcomes."
                )
            ),

            Section(
                "positioning",
                "Positioning Your Profile",
                Bullet(
                    "Make the target role obvious",
                    "Use your headline, summary, and recent work to point at one clear direction.",
                    "Mirror the language used in job descriptions without stuffing keywords.",
                    "Show the level you are aiming for through scope, ownership, and measurable outcomes."
                ),

                Bullet(
                    "Build proof recruiters can scan",
                    "Add 2-3 concrete projects or achievements.",
                    "Write short notes on decisions, trade-offs, and results.",
                    "Keep links, screenshots, and repositories clean enough to evaluate quickly."
                )
            ),

            Section(
                "execution",
                "Practical Action Plan",
                Check(
                    "Choose one target role and collect 8-10 representative job descriptions.",
                    "List the repeated skills, tools, and outcomes those roles ask for.",
                    "Update your resume and profile with matching evidence.",
                    "Create one focused project or case study that proves the missing signal.",
                    "Ask a mentor or experienced peer to review the story before applying."
                )
            ),

            Section(
                "mistakes",
                "Common Mistakes To Avoid",
                Check(
                    "Trying to look relevant for every role at once.",
                    "Listing tools without explaining impact.",
                    "Using generic summaries that do not match the target role.",
                    "Sending applications before your proof points are easy to understand.",
                    "Ignoring feedback after the first few rejections."
                )
            ),

            Section(
                "mentor-support",
                "How XQARE Mentors Help",
                Paragraph(
                    "XQARE mentors help turn broad career advice into a role-specific plan. They can review your profile, calibrate your level, identify missing proof, and help you practice the conversations that decide whether opportunities move forward."
                ),
                Quote(
                    $"A strong {article.CategoryName.ToLowerInvariant()} plan should make the next step feel specific: what to learn, what to build, what to show, and who to talk to."
                )
            )
        ];

        private static IReadOnlyList<CareerInsightSection> BuildFaangSections() =>
        [
            Section(
                "truth",
                "The Truth About FAANG Hiring",
                Paragraph(
                    "FAANG (or MAANG, or whatever the acronym is this year) hiring isn't a mystery. It's a repeatable process with well-understood mechanics. Candidates who treat it as a game with rules dramatically outperform candidates who treat it as a mystery to solve."
                ),
                Paragraph(
                    "This playbook is written by people who've been on both sides of the interview table at Meta, Amazon, Google, Microsoft, and Apple — many of whom are XQARE mentors.."
                )
            ),

            Section(
                "positioning",
                "Phase 1: Positioning Yourself (Weeks -12 to -8)",
                Paragraph("Before you apply, make yourself a candidate they want."),

                Bullet(
                    "Fix Your Resume for FAANG Standards",
                    "One page. Two if you have 8+ years.",
                    "Every bullet quantified: reduced p99 latency by 43% by migrating to gRPC across 4 services handling 12M daily requests.",
                    "No jargon soup. Clarity signals seniority.",
                    "Match your role level to your resume tone."
                ),

                Bullet(
                    "Build a Signal-Rich GitHub / Portfolio",
                    "2-3 substantive projects, not 20 forks.",
                    "Clean READMEs with architecture diagrams.",
                    "One well-written blog post about a technical decision you made."
                ),

                Bullet(
                    "Get Referrals",
                    "Reach out to XQARE mentors and Circle members who work there.",
                    "Warm, specific asks: I am targeting an L4 SWE role. Would you refer me?",
                    "Send them your resume and a 3-line pitch they can use."
                )
            ),

            Section(
                "recruiter",
                "Phase 2: Recruiter Screen (Week 0)",
                Paragraph(
                    "The recruiter's job is to gate-keep. Yours is to signal seriousness and level."
                ),
                Check(
                    "Answer level-fit questions with specifics.",
                    "Do not disclose current compensation.",
                    "State a specific target range based on levels.fyi and mentor input.",
                    "Ask about the timeline, panel composition, and specific team."
                )
            ),

            Section(
                "technical-screen",
                "Phase 3: The Phone/Video Technical Screen",
                Paragraph(
                    "Usually 45-60 minutes, one medium DSA problem or two easy-mediums."
                ),
                Check(
                    "Communicate before coding: restate the problem, discuss edge cases.",
                    "Start with brute force, then optimize.",
                    "Talk through trade-offs as you code.",
                    "Test your solution manually at the end.",
                    "Ask a great question to close."
                )
            ),

            Section(
                "onsite",
                "Phase 4: The Onsite / Virtual Onsite",
                Paragraph("Typically 4-5 rounds."),

                Bullet(
                    "Round 1-2: Coding (DSA)",
                    "Medium-hard problems.",
                    "Focus: correctness, communication, complexity analysis, testing.",
                    "Prep: 100-150 problems across patterns; NeetCode 150 is the gold list."
                ),

                Bullet(
                    "Round 3: System Design (mid+ candidates only)",
                    "Focus: framework, trade-offs, depth on 1-2 components, scalability.",
                    "Prep: 12-15 classic designs plus strong primitives."
                ),

                Bullet(
                    "Round 4: Behavioral / Leadership",
                    "Amazon: strict on Leadership Principles.",
                    "Google: probes for Googliness, ambiguity, collaboration.",
                    "Meta: cross-functional depth, impact stories.",
                    "Apple: deep craft, quiet confidence, opinionated but not dogmatic."
                ),

                Bullet(
                    "Round 5: Domain / Bar Raiser",
                    "A senior engineer or Bar Raiser probes depth.",
                    "Focus: judgment, seniority signal, growth mindset."
                )
            ),

            Section(
                "signals",
                "The 4 Signals They're Grading",
                Paragraph("Every FAANG interview is scored on some combination of:"),
                Signals(
                    ("Problem-Solving", "how you approach unfamiliar problems"),
                    ("Coding / Design", "quality of your solution"),
                    ("Communication", "clarity, structure, collaboration"),
                    ("Culture / Values", "fit for the specific company")
                ),
                Paragraph("Weak in one? You can still pass. Weak in two? You will fail.")
            ),

            Section(
                "mistakes",
                "Common Mistakes That Kill Otherwise Strong Candidates",
                Check(
                    "Diving into code before clarifying signals junior thinking.",
                    "Not testing your solution is a huge red flag.",
                    "Underprepping behavioral is assumed to be easy, actually rigorous.",
                    "Sharing offers you do not have means recruiters check, always.",
                    "Poor negotiation can leave 15-40% to the offer."
                )
            ),

            Section(
                "framework",
                "The FAANG Prep Framework: 12 Weeks",
                Check(
                    "Weeks 1-2: Baseline assessment, DSA gap analysis, story bank draft.",
                    "Weeks 3-6: Deliberate DSA plus 1 system design/week plus 1 mock/week.",
                    "Weeks 7-9: Company-specific prep, targeted problems, behavioral polish.",
                    "Weeks 10-11: Full mock loops, 2-3 per week.",
                    "Week 12: Rest, review, and interview."
                )
            ),

            Section(
                "outcomes",
                "How XQARE FAANG Mentors Change Outcomes",
                Paragraph("XQARE has active mentors at all major FAANG companies. They provide:"),
                Check(
                    "Level calibration: are you actually ready for L4, or should you target L3?",
                    "Company-specific prep: Amazon-style interviews are meaningfully different from Google-style.",
                    "Mock interviews that mirror the exact format, cadence, and depth of the real thing.",
                    "Behavioral coaching that flips outcomes for many candidates.",
                    "Referrals from mentors who know you personally.",
                    "Negotiation coaching that often adds meaningful value to the final offer."
                ),
                Quote(
                    "I did 6 mock interviews with my XQARE mentor. In three of them, he asked variations of what I got in the real interviews. Offer came back at L5 first try. — XQARE Member"
                )
            )
        ];

        private static IReadOnlyList<CareerInsightSection> BuildLinkedInSections() =>
        [
            Section(
                "why-linkedin-matters",
                "Why LinkedIn Optimization Is Non-Negotiable",
                Paragraph(
                    "For most tech professionals, LinkedIn is not just a resume — it's a passive job engine. Optimized profiles surface in recruiter searches, inbound message queues, and algorithmic feeds. Unoptimized profiles are invisible."
                ),
                Paragraph(
                    "The difference between an optimized and unoptimized LinkedIn for the same person can be 10x more inbound opportunities. And unlike a resume, you optimize it once and benefit for years."
                )
            ),

            Section(
                "profile-anatomy",
                "The Anatomy of a High-Performing LinkedIn Profile",

                Bullet(
                    "1. Profile Photo",
                    Item(Bold("Clear face, good lighting, professional but not stiff")),
                    Item(
                        Bold("Solid or blurred background"),
                        Text(" — no vacation photos")
                    ),
                    Item(
                        Bold("Smile with your eyes"),
                        Text(" — engagement rate matters")
                    ),
                    Item(Bold("Profiles with photos get 21x more views"))
                ),

                Bullet(
                    "2. Banner Image",
                    Item(Bold("Free real estate most people waste")),
                    Item(
                        Bold("Options:"),
                        Text(" company logo + tagline, key skills, or a clean personal brand image")
                    ),
                    Item(
                        Bold("Avoid generic stock photos of code;"),
                        Text(" recruiters see through them.")
                    )
                ),

                Bullet(
                    "3. Headline",
                    Item(
                        Bold("Your headline is a search-keyword battlefield, not just your job title.")
                    ),
                    Item(
                        Bold("Include your current role, two or three role-specific skills, and a differentiator.")
                    ),
                    Item(
                        Bold("Write for the next role you want, while remaining honest about your current scope.")
                    )
                )
            ),

            Section(
                "about",
                "Write an About Section Recruiters Actually Read",
                Paragraph(
                    "The first three lines are critical because LinkedIn truncates the rest. Open with a specific hook rather than a generic statement about being passionate."
                ),

                Check(
                    Item(
                        Bold("State what you do"),
                        Text(" and the problems you solve.")
                    ),
                    Item(
                        Bold("Name the technologies or domain"),
                        Text(" where you have real depth.")
                    ),
                    Item(Bold("Add one or two quantified proof points.")),
                    Item(
                        Bold("End with a clear, low-pressure way to contact you.")
                    )
                )
            ),

            Section(
                "featured",
                "Make Your Featured Section Work",
                Paragraph(
                    "Recruiters skim this area for proof. Use it to make your work easier to evaluate."
                ),

                Check(
                    Item(Bold("Pin your best project, case study, or technical blog post.")),
                    Item(Bold("Add a talk, portfolio, or strong GitHub repository.")),
                    Item(
                        Bold("Use a concise caption"),
                        Text(" that explains the decision, impact, or learning behind each item.")
                    ),
                    Item(
                        Bold("Remove outdated links"),
                        Text(" that no longer represent your direction.")
                    )
                )
            ),

            Section(
                "experience",
                "Turn Experience Into Evidence",
                Paragraph(
                    "Every role needs more than a responsibility list. Show scope, decisions, and measurable impact."
                ),

                Check(
                    Item(
                        Bold("Write three to five quantified bullets"),
                        Text(" for each meaningful role.")
                    ),
                    Item(
                        Bold("Lead with outcomes"),
                        Text(" before listing tools.")
                    ),
                    Item(
                        Bold("Use rich media"),
                        Text(" for recent projects when it adds proof.")
                    ),
                    Item(
                        Bold("Add a one-line company or product description"),
                        Text(" when the context is not obvious.")
                    )
                )
            ),

            Section(
                "skills",
                "Build a Searchable Skills Strategy",
                Paragraph(
                    "LinkedIn search relies heavily on skills. Add the full set, then place the most relevant skills near the top."
                ),

                Check(
                    Item(Bold("Add up to 50 relevant skills, not filler.")),
                    Item(
                        Bold("Front-load skills"),
                        Text(" that match your target roles.")
                    ),
                    Item(
                        Bold("Ask trusted senior contacts"),
                        Text(" for meaningful endorsements.")
                    ),
                    Item(
                        Bold("Refresh the list"),
                        Text(" whenever your target role or stack changes.")
                    )
                )
            ),

            Section(
                "keywords",
                "The Keyword Strategy",
                Paragraph(
                    "Recruiters search LinkedIn like a database. Your goal is to appear in the right searches without keyword stuffing."
                ),

                Check(
                    Item(
                        Bold("Review real job descriptions"),
                        Text(" for your target roles.")
                    ),
                    Item(
                        Bold("Extract recurring job titles, skills, tools, and outcomes.")
                    ),
                    Item(
                        Bold("Use them naturally"),
                        Text(" in your headline, About, experience, and skills.")
                    ),
                    Item(
                        Bold("Include both acronyms and their full terms"),
                        Text(" when relevant.")
                    )
                )
            ),

            Section(
                "activity",
                "Activity: The Multiplier Most People Ignore",
                Paragraph(
                    "An optimized profile is passive; visible activity multiplies its reach."
                ),

                Signals(
                    ("One substantive post each week", "share a learning, project, insight, or informed point of view"),
                    ("Comment thoughtfully", "add useful context to five to ten relevant posts each week"),
                    ("Share your proof", "publish project wins, technical notes, and blog posts"),
                    ("Follow hiring conversations", "engage with content from target companies and leaders")
                )
            ),

            Section(
                "open-to-work",
                "The Open to Work Setting",
                Paragraph(
                    "Use recruiter-only visibility when you are employed and want a discreet search. The public badge is best reserved for situations where broad visibility is useful."
                )
            ),

            Section(
                "sprint",
                "The 60-Minute LinkedIn Optimization Sprint",

                Signals(
                    ("Minutes 0-10", "update your photo, banner, and custom URL"),
                    ("Minutes 10-20", "rewrite the headline with target-role keywords"),
                    ("Minutes 20-35", "rewrite the About section around proof and direction"),
                    ("Minutes 35-50", "add skills, recommendations, and featured work"),
                    ("Minutes 50-60", "set intent, review keywords, and publish one useful post")
                )
            ),

            Section(
                "mentor-support",
                "How XQARE Mentors Elevate Your Profile",
                Paragraph(
                    "XQARE mentors help you turn profile edits into a credible career story. They audit your profile through a recruiter lens, rewrite your headline and About section, identify the skills that matter for your target roles, and coach the content that creates inbound opportunities."
                ),
                Quote(
                    "A strong LinkedIn profile does not try to impress everyone. It makes the right next opportunity obvious."
                )
            )
        ];

        private static IReadOnlyList<CareerInsightSection> BuildMentorshipSections() =>
[
    Section(
        "why-mentorship",
        "Why Mentorship Is the Highest ROI Career Investment",

        Paragraph(
            "In the fast-paced world of technology, having a mentor can be the difference between years of trial-and-error and a focused, accelerated career path. Multiple studies — from Sun Microsystems' internal research to Harvard Business Review's 2024 workplace survey — show that mentored professionals are promoted up to 5x more often, earn 20-30% more over a decade, and report significantly higher career satisfaction."
        ),

        Paragraph(
            "Yet 76% of tech professionals say they've never had a real mentor. The gap isn't awareness — it's access. XQARE was built to close that exact gap."
        )
    ),

    Section(
        "great-mentor",
        "What a Great Mentor Actually Gives You",

        Paragraph(
            "A mentor isn't a coach, a manager, or a tutor. A mentor is someone who has already walked the path you're on and can give you the map. Specifically, they provide:"
        ),

        Check(
            Item(
                Bold("Industry knowledge"),
                Text(" — that isn't found in textbooks — the political dynamics, unspoken norms, and cultural nuances of specific companies and teams")
            ),

            Item(
                Bold("Network access"),
                Text(" — to decision-makers, hiring managers, and rare opportunities most jobs never get posted")
            ),

            Item(
                Bold("Accountability"),
                Text(" — to keep you honest with the goals you set for yourself when motivation runs out")
            ),

            Item(
                Bold("Perspective"),
                Text(" — from someone who has already made the mistakes you're about to make")
            ),

            Item(
                Bold("Pattern recognition"),
                Text(" — they've seen dozens of engineers navigate the same crossroads and know which paths actually lead where")
            )
        )
    ),

    Section(
        "finding-right-mentor",
        "Finding the Right Mentor",

        Paragraph(
            "Not every experienced professional makes a great mentor. Seniority is not mentorship. Look for someone who:"
        ),

        Bullet(
            "1. Has genuine interest in your growth",
            Item(
                Bold("Has genuine interest in your growth"),
                Text(" — they ask questions instead of just giving speeches")
            )
        ),

        Bullet(
            "2. Works in or understands your target domain",
            Item(
                Bold("Works in or understands your target domain"),
                Text(" — advice from a mobile engineer won't help an ML aspirant")
            )
        ),

        Bullet(
            "3. Communicates clearly and consistently",
            Item(
                Bold("Communicates clearly and consistently"),
                Text(" — mentorship dies in inconsistent scheduling")
            )
        ),

        Bullet(
            "4. Challenges you while being supportive",
            Item(
                Bold("Challenges you while being supportive"),
                Text(" — a mentor who only agrees with you is worthless")
            )
        ),

        Bullet(
            "5. Is 3-7 years ahead of you",
            Item(
                Bold("Is 3-7 years ahead of you"),
                Text(" — much further and they've forgotten what your stage feels like")
            )
        )
    ),

    Section(
        "xqare-matching",
        "How XQARE Matches You With the Right Mentor",

        Paragraph(
            "XQARE's mentorship engine uses AI-powered matching based on:"
        ),

        Check(
            Item(
                Bold("Career goals"),
                Text(" — where you want to be in 2-3 years")
            ),

            Item(
                Bold("Current stack and role"),
                Text(" — so advice maps to your reality")
            ),

            Item(
                Bold("Learning style"),
                Text(" — some people need direct answers, others need Socratic questioning")
            ),

            Item(
                Bold("Communication cadence"),
                Text(" — weekly, bi-weekly, or async-first")
            )
        ),

        Paragraph(
            "Every mentor on XQARE has been vetted for their expertise and their commitment to giving back. They're not selling courses. They're not building funnels. They're professionals who remember what it felt like to need guidance and want to pay it forward."
        )
    ),

    Section(
        "getting-most-mentorship",
        "Getting the Most Out of Mentorship",

        Paragraph(
            "The single biggest predictor of mentorship success isn't the mentor — it's the mentee. Here's how top XQARE members consistently extract 10x value:"
        ),

        Bullet(
            "Before the Session",

            Item(
                Text("Come prepared with "),
                Bold("2-3 specific questions"),
                Text(", not \"let's just chat\"")
            ),

            Item(
                Text("Share context in advance — what you're working on, what's stuck")
            ),

            Item(
                Text("Set one measurable outcome for the session")
            )
        ),

        Bullet(
            "During the Session",

            Item(
                Text("Take notes yourself — don't rely on memory")
            ),

            Item(
                Text("Push back when advice doesn't fit your situation")
            ),

            Item(
                Text("Ask \"what would you do differently if you were me?\"")
            )
        ),

        Bullet(
            "After the Session",

            Item(
                Text("Act on at least one piece of feedback within 7 days")
            ),

            Item(
                Text("Send a short recap message showing you internalized the advice")
            ),

            Item(
                Text("Share progress in the next session — mentors invest more in mentees who show movement")
            )
        )
    ),

    Section(
        "mentor-quote",
        "Mentor Perspective",

        Quote(
            "\"The best mentors don't give you answers — they help you ask better questions.\" — XQARE Mentor, Senior Staff Engineer."
        )
    ),

    Section(
        "real-outcomes",
        "Real Outcomes From XQARE Members",

        Check(
            Item(
                Bold("Rahul, backend engineer"),
                Text(" — went from stuck at mid-level for 4 years to a Staff role at a series-B startup in 11 months after his XQARE mentor helped him reframe his portfolio around business impact instead of technical complexity")
            ),

            Item(
                Bold("Anjali, data analyst"),
                Text(" — transitioned into ML engineering in 8 months with a personalized learning roadmap her mentor built with her, then referred her to her first ML role")
            ),

            Item(
                Bold("Karthik, fresher"),
                Text(" — landed 3 offers from tier-1 product companies as a college senior after 6 months of structured mock interviews with XQARE mentors")
            )
        )
    ),

    Section(
        "start-today",
        "Start Today",

        Paragraph(
            "Your next career breakthrough might be one conversation away. Join XQARE, complete your profile, and get matched with mentors who've already been where you want to go."
        )
    )
];

        private static IReadOnlyList<CareerInsightSection> BuildCloudSections() =>
[
    Section(
        "cloud-landscape",
        "The Cloud Engineering Landscape in 2026",

        Paragraph(
            "Cloud computing continues to be one of the fastest-growing fields in technology. The Bureau of Labor Statistics projects 22% growth through 2030 — nearly 4x the average across all professions. Companies of all sizes are migrating to the cloud, and the shortage of skilled cloud engineers has pushed average salaries in India to ₹18-45 LPA and $130k-$210k globally."
        ),

        Paragraph(
            "But \"cloud engineer\" is a vague title. Before you start, understand that the field splits into three broad tracks:"
        ),

        Check(
            Item(
                Bold("Cloud Infrastructure / DevOps"),
                Text(" — building and operating the systems apps run on")
            ),
            Item(
                Bold("Cloud Security"),
                Text(" — securing workloads, identity, and data in the cloud")
            ),
            Item(
                Bold("Cloud Solutions Architecture"),
                Text(" — designing systems that use cloud services well")
            )
        ),

        Paragraph(
            "This roadmap gets you to the point where you can pick a track with confidence."
        )
    ),

    Section(
        "step-by-step-roadmap",
        "Step-by-Step Roadmap",

        Signals(
            ("1. Master the Fundamentals (Months 0-3)",
             "Skip this and everything above collapses. You cannot debug cloud systems without understanding what they abstract."),

            ("2. Learn ONE Major Cloud Platform Deeply (Months 3-8)",
             "The biggest mistake beginners make is trying to learn AWS, Azure, and GCP simultaneously. Pick one. Once you know one deeply, the others become 40% familiar automatically."),

            ("3. Get Certified — Strategically (Months 6-10)",
             "Certifications don't make you an engineer, but they get your resume past filters and force you to learn breadth you'd otherwise skip."),

            ("4. Build Real Projects (Months 6-12, in parallel)",
             "Certifications get interviews. Projects get offers. Build at least 3 portfolio projects that show progression:"),

            ("5. Specialize (Months 12+)",
             "Now you can commit to a track with real information about what you enjoy:")
        )
    ),

    Section(
        "fundamentals",
        "Master the Fundamentals",

        Paragraph(
            "Skip this and everything above collapses. You cannot debug cloud systems without understanding what they abstract."
        ),

        Check(
            Item(
                Bold("Networking basics"),
                Text(" — TCP/IP, DNS, HTTP/HTTPS, load balancing, subnets, CIDR blocks")
            ),
            Item(
                Bold("Linux system administration"),
                Text(" — file permissions, processes, systemd, log analysis")
            ),
            Item(
                Bold("Basic scripting"),
                Text(" — Python for automation, Bash for glue code")
            ),
            Item(
                Bold("Version control"),
                Text(" — Git branching, PR workflows, rebasing")
            )
        )
    ),

    Section(
        "cloud-platform",
        "Learn ONE Major Cloud Platform Deeply",

        Paragraph(
            "The biggest mistake beginners make is trying to learn AWS, Azure, and GCP simultaneously. Pick one. Once you know one deeply, the others become 40% familiar automatically."
        ),

        Check(
            Item(
                Bold("AWS"),
                Text(" — Most widely adopted, largest job market, best starting choice for most people")
            ),
            Item(
                Bold("Azure"),
                Text(" — Strong enterprise and hybrid cloud presence, great if you're targeting Fortune 500s")
            ),
            Item(
                Bold("GCP"),
                Text(" — Growing rapidly, dominant in data/ML workloads and Kubernetes-first shops")
            )
        ),

        Paragraph(
            "Focus on the core services first: compute (EC2/VMs), storage (S3/Blob), networking (VPC/VNet), identity (IAM), and one managed database."
        )
    ),

    Section(
        "certifications",
        "Get Certified — Strategically",

        Paragraph(
            "Certifications don't make you an engineer, but they get your resume past filters and force you to learn breadth you'd otherwise skip."
        ),

        Check(
            Item(
                Bold("AWS Solutions Architect Associate"),
                Text(" — the gold standard entry cert")
            ),
            Item(
                Bold("Azure Administrator Associate (AZ-104)"),
                Text(" — Azure equivalent")
            ),
            Item(
                Bold("Google Cloud Associate Cloud Engineer"),
                Text(" — GCP entry point")
            ),
            Item(
                Bold("HashiCorp Terraform Associate"),
                Text(" — cloud-agnostic infrastructure-as-code")
            )
        )
    ),

    Section(
        "real-projects",
        "Build Real Projects",

        Paragraph(
            "Certifications get interviews. Projects get offers. Build at least 3 portfolio projects that show progression:"
        ),

        Check(
            Item(
                Bold("Project 1"),
                Text(" — Deploy a 3-tier web application manually — learn the pain")
            ),
            Item(
                Bold("Project 2"),
                Text(" — Rebuild it using Terraform + CI/CD pipeline — feel the productivity gain")
            ),
            Item(
                Bold("Project 3"),
                Text(" — Add observability (metrics, logs, tracing), autoscaling, and a disaster recovery plan")
            )
        ),

        Paragraph(
            "Push all of them to GitHub with clear READMEs, architecture diagrams, and a blog post explaining the design decisions. This portfolio alone lands interviews."
        )
    ),

    Section(
        "specialize",
        "Specialize",

        Paragraph(
            "Now you can commit to a track with real information about what you enjoy:"
        ),

        Check(
            Item(
                Bold("DevOps & SRE"),
                Text(" — reliability, incident response, deployment velocity")
            ),
            Item(
                Bold("Cloud Security"),
                Text(" — IAM, network security, compliance frameworks")
            ),
            Item(
                Bold("Data Engineering on Cloud"),
                Text(" — pipelines, warehouses, lakehouses")
            ),
            Item(
                Bold("Serverless Architecture"),
                Text(" — event-driven systems, functions-as-a-service")
            ),
            Item(
                Bold("Platform Engineering"),
                Text(" — internal developer platforms and golden paths")
            )
        )
    ),

    Section(
        "common-mistakes",
        "Common Mistakes to Avoid",

        Check(
            Item(
                Bold("Certification-only strategy"),
                Text(" — a resume with 4 certs and zero projects screams \"I studied but never built\"")
            ),
            Item(
                Bold("Tutorial hell"),
                Text(" — passively watching courses without building anything")
            ),
            Item(
                Bold("Ignoring cost management"),
                Text(" — one accidentally-public S3 bucket or a forgotten GPU instance can cost you thousands")
            ),
            Item(
                Bold("Skipping Linux"),
                Text(" — cloud is Linux at scale; you can't skip the base layer")
            ),
            Item(
                Bold("Job-hopping too early"),
                Text(" — the first cloud role teaches you 10x more than the second")
            )
        )
    ),

    Section(
        "xqare-journey",
        "How XQARE Accelerates Your Journey",

        Paragraph(
            "XQARE cloud engineering mentors"
        ),

        Check(
            Item(
                Bold("Audit your roadmap"),
                Text(" — in the first session so you don't waste months on the wrong sequence")
            ),
            Item(
                Bold("Review your portfolio projects"),
                Text(" — with the eye of someone who actually hires cloud engineers")
            ),
            Item(
                Bold("Prep for cloud-specific interviews"),
                Text(" — Well-Architected reviews, cost-optimization case studies, incident post-mortems")
            ),
            Item(
                Bold("Get referred"),
                Text(" — into hiring pipelines at cloud-first companies")
            )
        ),

        Quote(
            "\"I followed a random YouTube roadmap for 8 months and got no interviews. My XQARE mentor tore it up in one session, rebuilt it around what employers actually test for, and I had 3 offers within 14 weeks.\" — XQARE Member, Cloud Engineer at Razorpay"
        )
    )
];

        private static IReadOnlyList<CareerInsightSection> BuildHiringSkillsSections() =>
[
    Section(
        "what-employers-are-paying-for-2026",
        "What Employers Are Actually Paying For in 2026",

        Paragraph(
            "The tech industry evolves rapidly, but hiring patterns move slower than trends. Here's what we're seeing across the 400+ hiring partners in the XQARE network — based on real job postings, offer data, and hiring manager interviews from Q4 2025 and Q1 2026."
        )
    ),

    Section(
        "technical-skills-highest-demand",
        "Technical Skills In Highest Demand",

        Bullet(
            "1. Applied AI & Machine Learning",

            Item(
                Text("Not just researchers — companies want people who can ship AI-powered features into real products.")
            ),

            Item(
                Bold("LLM fine-tuning and RAG systems"),
                Text(" — retrieval-augmented generation is the highest-growth skill")
            ),

            Item(
                Bold("Prompt engineering + evaluation"),
                Text(" — measuring model quality at scale")
            ),

            Item(
                Bold("MLOps"),
                Text(" — deploying, monitoring, and retraining models in production")
            ),

            Item(
                Bold("AI safety & guardrails"),
                Text(" — as regulation catches up, this becomes non-optional")
            )
        ),

        Bullet(
            "2. Cloud Architecture & DevOps",

            Item(
                Text("Every company is a cloud company now — even the ones that don't know it yet.")
            ),

            Item(
                Text("Multi-cloud and hybrid architectures")
            ),

            Item(
                Text("Kubernetes and container orchestration")
            ),

            Item(
                Text("Infrastructure-as-Code (Terraform, Pulumi)")
            ),

            Item(
                Bold("FinOps"),
                Text(" — cloud cost optimization is a full-time role at scale")
            )
        ),

        Bullet(
            "3. Cybersecurity",

            Item(
                Text("Cyber attacks grew 38% year-over-year. Every hire is now a scarce hire.")
            ),

            Item(
                Text("Application security (AppSec) — securing code, not just networks")
            ),

            Item(
                Text("Cloud security posture management")
            ),

            Item(
                Text("Zero-trust architecture")
            ),

            Item(
                Text("Threat detection with SIEM/SOAR platforms")
            )
        ),

        Bullet(
            "4. Full-Stack Development",

            Item(
                Text("Still the backbone of tech hiring — but the bar has risen.")
            ),

            Item(
                Bold("React / Next.js"),
                Text(" — for frontend")
            ),

            Item(
                Bold("Node.js, Python, or Go"),
                Text(" — for backend")
            ),

            Item(
                Text("Comfortable with SQL AND at least one NoSQL DB")
            ),

            Item(
                Text("Basic cloud deployment fluency (Vercel, AWS, Cloudflare)")
            )
        ),

        Bullet(
            "5. Data Engineering",

            Item(
                Text("Every AI initiative depends on the data layer beneath it.")
            ),

            Item(
                Text("Pipeline design and orchestration (Airflow, Dagster, dbt)")
            ),

            Item(
                Text("Real-time streaming (Kafka, Flink)")
            ),

            Item(
                Text("Warehouse mastery (Snowflake, BigQuery, Databricks)")
            ),

            Item(
                Text("Data quality and observability")
            )
        )
    ),

    Section(
        "soft-skills",
        "Soft Skills That Now Separate Offers From Rejections",

        Paragraph(
            "Every hiring manager XQARE has interviewed in 2026 flagged the same pattern: technical skills get you the interview, soft skills close the offer."
        ),

        Check(
            Item(
                Bold("Communication"),
                Text(" — writing crisp docs, articulating trade-offs, disagreeing productively")
            ),

            Item(
                Bold("Ownership"),
                Text(" — treating problems as yours until they're solved, not passing them to the next team")
            ),

            Item(
                Bold("Adaptability"),
                Text(" — the average tech stack shelf-life is now 3 years; learners win")
            ),

            Item(
                Bold("Cross-functional empathy"),
                Text(" — working with PMs, designers, and business teams without friction")
            ),

            Item(
                Bold("Mentoring instinct"),
                Text(" — even at junior levels, teaching signals seniority")
            )
        )
    ),

    Section(
        "whats-declining",
        "What's Declining",

        Paragraph(
            "Being honest with candidates matters. These skills are still useful but no longer differentiators:"
        ),

        Check(
            Item(
                Text("Standalone jQuery / Angular.js legacy work")
            ),

            Item(
                Text("Purely on-premise infrastructure roles")
            ),

            Item(
                Text("Waterfall-only project management")
            ),

            Item(
                Text("Manual QA without any automation exposure")
            )
        )
    ),

    Section(
        "profile-everyone-wants",
        "The Profile Everyone Wants to Hire",

        Paragraph(
            "If we had to describe the perfect 2026 hire in one line:"
        ),

        Paragraph(
            "A pragmatic full-stack builder with one deep specialty, one AI-adjacent skill, and clear communication."
        ),

        Paragraph(
            "That means:"
        ),

        Check(
            Item(
                Text("You can build end-to-end features")
            ),

            Item(
                Text("You've gone deep on one area (data, security, infra, ML, mobile)")
            ),

            Item(
                Text("You've integrated AI into at least one shipped product")
            ),

            Item(
                Text("You can explain your work to a non-technical stakeholder")
            )
        )
    ),

    Section(
        "xqare-keeps-you-ahead",
        "How XQARE Keeps You Ahead of the Curve",

        Paragraph(
            "Trends become saturated the moment they hit YouTube. XQARE mentors — who work at the companies setting these trends — help you:"
        ),

        Check(
            Item(
                Bold("Spot rising skills 6-12 months before they're mainstream")
            ),

            Item(
                Bold("Prune skills that are quietly declining"),
                Text(" — so you don't over-invest")
            ),

            Item(
                Bold("Build a personalized learning roadmap"),
                Text(" — that matches your target companies' actual JDs")
            ),

            Item(
                Bold("Connect with hiring partners"),
                Text(" — actively looking for your emerging skill set")
            )
        ),

        Quote(
            "\"My XQARE mentor told me to start learning RAG systems in mid-2024 when nobody was hiring for it. By late 2025, I was one of the few candidates with real experience and I picked from 4 offers.\" — XQARE Member"
        )
    )
];

        private static IReadOnlyList<CareerInsightSection> BuildTechnicalInterviewSections() =>
[
    Section(
        "real-reason-candidates-fail",
        "The Real Reason Most Candidates Fail Interviews",

        Paragraph(
            "Technical interviews are not a test of intelligence. They're a test of how well you can think, communicate, and stay composed under artificial time pressure. Once you understand that, prep becomes strategic instead of frantic."
        ),

        Paragraph(
            "Most candidates spend 400+ hours on LeetCode and still fail — because they're optimizing one dimension of a multi-dimensional evaluation."
        )
    ),

    Section(
        "four-pillars",
        "The Four Pillars of Interview Prep",

        Bullet(
            "1. Data Structures & Algorithms (DSA)",

            Item(
                Text("The floor, not the ceiling.")
            ),

            Item(
                Bold("Master patterns, not problems"),
                Text(" — sliding window, two pointers, BFS/DFS, dynamic programming, backtracking, heaps, tries")
            ),

            Item(
                Bold("Aim for ~150 problems total, not 500"),
                Text(" — focused reps on patterns beats mindless volume")
            ),

            Item(
                Bold("Practice explaining out loud"),
                Text(" — silent problem-solving trains the wrong muscle")
            ),

            Item(
                Text("Use LeetCode, NeetCode, or the XQARE curated problem lists tailored by mentor recommendation")
            )
        ),

        Bullet(
            "2. System Design (for mid+ roles)",

            Item(
                Text("Where senior offers are made or lost.")
            ),

            Item(
                Bold("Study scalability primitives"),
                Text(" — load balancers, caching, sharding, replication, CDN")
            ),

            Item(
                Bold("Study classic designs"),
                Text(" — URL shortener, chat app, news feed, ride-sharing, rate limiter")
            ),

            Item(
                Bold("Learn the 4-step framework"),
                Text(" — clarify requirements → high-level design → deep dive → wrap up")
            ),

            Item(
                Bold("Understand trade-offs"),
                Text(" — CAP theorem, consistency models, sync vs async, SQL vs NoSQL")
            )
        ),

        Bullet(
            "3. Behavioral Interviews",

            Item(
                Text("Underrated by candidates, decisive for hiring managers.")
            ),

            Item(
                Bold("Use the STAR method"),
                Text(" — Situation, Task, Action, Result")
            ),

            Item(
                Bold("Prepare 8-10 core stories"),
                Text(" you can flex into any question (\"tell me about a conflict,\" \"a time you failed,\" \"your biggest impact\")")
            ),

            Item(
                Bold("Quantify outcomes wherever possible"),
                Text(" — \"reduced deploy time by 40%\"")
            ),

            Item(
                Bold("Match your stories to the company's leadership principles"),
                Text(" — Amazon's LPs, Google's Googleyness, etc.")
            )
        ),

        Bullet(
            "4. Domain / Stack Depth",

            Item(
                Text("The tiebreaker.")
            ),

            Item(
                Bold("Be razor sharp"),
                Text(" on the technologies listed on your resume — nothing kills faster than \"I put React on my resume but can't explain hooks\"")
            ),

            Item(
                Bold("Know one layer below"),
                Text(" what you use daily — if you write React, know how the reconciler works")
            ),

            Item(
                Bold("Have strong opinions"),
                Text(" on trade-offs in your stack")
            )
        )
    ),

    Section(
        "twelve-week-framework",
        "The 12-Week Prep Framework",

        Bullet(
            "Weeks 1-2: Foundation Assessment",

            Item(
                Text("Solve 20 mixed-difficulty DSA problems to identify pattern gaps")
            ),

            Item(
                Text("Take a mock system design interview to establish your baseline")
            ),

            Item(
                Text("List 8-10 behavioral stories from your career")
            )
        ),

        Bullet(
            "Weeks 3-8: Deliberate Practice",

            Item(
                Bold("DSA"),
                Text(" — 2-3 problems per day, rotating across patterns")
            ),

            Item(
                Bold("System design"),
                Text(" — 1 design per week, written up with diagrams")
            ),

            Item(
                Bold("Behavioral"),
                Text(" — refine stories with feedback from a mentor")
            ),

            Item(
                Bold("1 mock interview per week"),
                Text(" — full 45-min pressure test")
            )
        ),

        Bullet(
            "Weeks 9-11: Company-Specific Prep",

            Item(
                Text("Study your target companies' interview formats (levels.fyi, Glassdoor, XQARE mentor debriefs)")
            ),

            Item(
                Text("Solve the problems tagged for each company on LeetCode")
            ),

            Item(
                Text("Prep company-specific behavioral answers (Amazon → LPs, Google → project depth)")
            )
        ),

        Bullet(
            "Week 12: Peak & Recovery",

            Item(
                Text("Reduce study intensity — you don't want to peak in prep, you want to peak in the interview")
            ),

            Item(
                Text("Sleep. Rest. Do 1-2 easy problems per day for confidence")
            ),

            Item(
                Text("Rehearse your intro and questions to ask the interviewer")
            )
        )
    ),

    Section(
        "interview-day-playbook",
        "Interview Day Playbook",

        Signals(
            ("1. Think out loud", "silent geniuses fail. Narrate your reasoning."),
            ("2. Clarify before coding", "3-5 minutes of questions is expected, not weak"),
            ("3. Discuss trade-offs", "every design choice has costs; name them"),
            ("4. Test your code manually", "walk through with an example before saying \"done\""),
            ("5. Ask great questions at the end", "this is a signal of seniority")
        )
    ),

    Section(
        "xqare-mentors",
        "How XQARE Mentors Change the Game",

        Paragraph(
            "Reading about interviews and doing them are different sports. XQARE mentors — many of whom actively conduct interviews at FAANG, Uber, Stripe, and top Indian unicorns — offer:"
        ),

        Check(
            Item(
                Bold("Mock rounds mirroring specific companies"),
                Text(" — the exact format, style, and follow-up depth")
            ),

            Item(
                Bold("Live feedback"),
                Text(" — on your communication, not just your solution")
            ),

            Item(
                Bold("Debriefs from real interviews"),
                Text(" — what actually gets asked in 2026")
            ),

            Item(
                Bold("Referrals"),
                Text(" — into their networks once you're ready")
            )
        ),

        Quote(
            "\"I did 3 mock system design sessions with my XQARE mentor. In the real interview, I hit the exact same trade-off discussion we'd rehearsed. Level 5 offer, first try.\" — XQARE Member"
        )
    )
];



        private static CareerInsightSection Section(
            string id,
            string title,
            params CareerInsightContentBlock[] blocks) =>
            new()
            {
                Id = id,
                Title = title,
                Blocks = blocks
            };

        private static CareerInsightContentBlock Paragraph(string text) =>
            new()
            {
                Type = CareerInsightBlockType.Paragraph,
                Text = text
            };

        private static CareerInsightContentBlock Quote(string text) =>
            new()
            {
                Type = CareerInsightBlockType.Quote,
                Text = text
            };

        private static CareerInsightContentBlock Check(params string[] items) =>
            new()
            {
                Type = CareerInsightBlockType.CheckList,
                Items = items
                    .Select(item => new CareerInsightRichItem
                    {
                        Spans =
                        [
                            new CareerInsightTextSpan
                            {
                                Text = item,
                                Bold = false
                            }
                        ]
                    })
                    .ToArray()
            };

        private static CareerInsightContentBlock Check(
            params CareerInsightRichItem[] items) =>
            new()
            {
                Type = CareerInsightBlockType.CheckList,
                Items = items
            };

        private static CareerInsightContentBlock Bullet(
            string heading,
            params string[] items) =>
            new()
            {
                Type = CareerInsightBlockType.BulletList,
                Heading = heading,
                Items = items
                    .Select(item => new CareerInsightRichItem
                    {
                        Spans =
                        [
                            new CareerInsightTextSpan
                            {
                                Text = item,
                                Bold = false
                            }
                        ]
                    })
                    .ToArray()
            };

        private static CareerInsightContentBlock Bullet(
            string heading,
            params CareerInsightRichItem[] items) =>
            new()
            {
                Type = CareerInsightBlockType.BulletList,
                Heading = heading,
                Items = items
            };

        private static CareerInsightRichItem Item(
            params CareerInsightTextSpan[] spans) =>
            new()
            {
                Spans = spans
            };

        private static CareerInsightTextSpan Text(string text) =>
            new()
            {
                Text = text,
                Bold = false
            };

        private static CareerInsightTextSpan Bold(string text) =>
            new()
            {
                Text = text,
                Bold = true
            };

        private static CareerInsightContentBlock Signals(
            params (string Label, string Description)[] signals) =>
            new()
            {
                Type = CareerInsightBlockType.NumberedSignals,
                Signals = signals
                    .Select(signal => new CareerInsightSignal
                    {
                        Label = signal.Label,
                        Description = signal.Description
                    })
                    .ToArray()
            };
    }
}