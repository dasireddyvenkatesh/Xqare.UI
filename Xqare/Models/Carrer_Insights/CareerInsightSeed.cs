namespace Xqare.Models.Carrer_Insights
{
    public class CareerInsightSeed
    {
        public static CareerInsightPageModel Build()
        {
            var categories = new[]
            {
            new CareerInsightCategory { Id = "all", Name = "All", IsDefault = true },
            new CareerInsightCategory { Id = "career-guidance", Name = "Career Guidance" },
            new CareerInsightCategory { Id = "mentorship", Name = "Mentorship & Professional Growth" },
            new CareerInsightCategory { Id = "industry", Name = "Industry Insights" },
            new CareerInsightCategory { Id = "job-preparation", Name = "Job Preparation" },
            new CareerInsightCategory { Id = "platform", Name = "XQARE Platform Updates" }
        };

            var articles = new List<CareerInsightArticle>
        {
            Article("genai-2026", "industry", "Industry Insights", "Generative AI Skills Every IT Professional Must Learn in 2026", "The 5 GenAI skills that actually get you hired in 2026: prompting, RAG, evaluation, agents, and production integration. Skip the hype, build what pays.", "https://images.unsplash.com/photo-1485827404703-89b55fcc595e?auto=format&fit=crop&w=1200&q=85", "Ishaan Verma", new DateTime(2026, 3, 10), 12, true, true, true, ["ai", "rag", "agents", "career"]),
            Article("faang-offer", "job-preparation", "Job Preparation", "Landing Your First FAANG Offer: The Insider Playbook", "A step-by-step playbook from XQARE mentors who've hired at Meta, Amazon, Google, and Microsoft. Learn the rules and prep rhythm.", "https://images.unsplash.com/photo-1529236183275-4fdcf2bc987e?auto=format&fit=crop&w=900&q=85", "Aditi Ramanathan", new DateTime(2026, 3, 9), 13, false, false, false, ["interviews", "faang", "offers"]),
            Article("linkedin-guide", "career-guidance", "Career Guidance", "The Ultimate LinkedIn Profile Optimization Guide for Tech Professionals", "A 60-minute LinkedIn optimization sprint that turns your profile into a passive job engine, with the exact sections recruiters scan first.", "https://images.unsplash.com/photo-1611944212129-29977ae1398c?auto=format&fit=crop&w=900&q=85", "Sanya Kapoor", new DateTime(2026, 3, 8), 10, false, false, false, ["linkedin", "personal brand"]),
            Article("mentorship-growth", "mentorship", "Mentorship & Professional Growth", "How Mentorship Can Accelerate Your Tech Career", "Discover how connecting with the right mentor can fast-track your growth, open doors to new opportunities, and sharpen your decisions.", "https://images.unsplash.com/photo-1552664730-d307ca884978?auto=format&fit=crop&w=900&q=85", "Priya Sharma", new DateTime(2026, 3, 5), 7, false, false, false, ["mentorship", "growth"]),
            Article("cloud-roadmap", "career-guidance", "Career Guidance", "Roadmap to Become a Cloud Engineer", "A step-by-step guide covering certifications, skills, and projects you need to land your first cloud engineering role.", "https://images.unsplash.com/photo-1446776811953-b23d57bd21aa?auto=format&fit=crop&w=900&q=85", "Arjun Mehta", new DateTime(2026, 3, 2), 6, false, false, false, ["cloud", "roadmap"]),
            Article("technical-interviews", "job-preparation", "Job Preparation", "How to Prepare for Technical Interviews", "Proven strategies and resources to help you ace coding interviews and system design rounds at top tech companies.", "https://images.unsplash.com/photo-1516321318423-f06f85e504b3?auto=format&fit=crop&w=900&q=85", "Neha Gupta", new DateTime(2026, 2, 28), 8, false, false, false, ["coding", "system design"]),
            Article("hiring-skills", "industry", "Industry Insights", "Top Skills Tech Companies Are Hiring For", "Stay ahead of the curve by learning which technical and soft skills are most in demand across the industry right now.", "https://images.unsplash.com/photo-1497366754035-f200968a6e72?auto=format&fit=crop&w=900&q=85", "Ravi Patel", new DateTime(2026, 2, 25), 5, false, false, false, ["skills", "hiring"]),
            Article("resume-tips", "job-preparation", "Job Preparation", "Resume Tips for IT Professionals", "Learn how to craft a compelling resume that stands out to recruiters and passes through applicant tracking systems.", "https://images.unsplash.com/photo-1517842645767-c639042777db?auto=format&fit=crop&w=900&q=85", "Meera Iyer", new DateTime(2026, 2, 20), 9, false, false, false, ["resume", "ats"]),
            Article("data-engineering", "career-guidance", "Career Guidance", "How to Transition into Data Engineering", "A practical guide for developers and analysts looking to pivot into one of the fastest-growing roles in tech.", "https://images.unsplash.com/photo-1551288049-bebda4e38f71?auto=format&fit=crop&w=900&q=85", "Karan Shah", new DateTime(2026, 2, 18), 11, false, false, false, ["data", "engineering"]),
            Article("fresher-tech-lead", "career-guidance", "Career Guidance", "From Fresher to Tech Lead: A Career Growth Blueprint", "Map out the key milestones, skills, and mindset shifts required to grow from your first tech role into technical leadership.", "https://images.unsplash.com/photo-1519389950473-47ba0277781c?auto=format&fit=crop&w=900&q=85", "Anika Rao", new DateTime(2026, 2, 15), 8, false, false, false, ["leadership", "growth"])
        };

            return new CareerInsightPageModel
            {
                Categories = categories,
                FeaturedArticle = articles.First(article => article.IsFeatured),
                Articles = articles
            };
        }

        private static CareerInsightArticle Article(
            string id,
            string categoryId,
            string categoryName,
            string title,
            string summary,
            string imageUrl,
            string author,
            DateTime publishedOn,
            int readMinutes,
            bool isTrending,
            bool isNew,
            bool isFeatured,
            IReadOnlyList<string> tags)
        {
            return new CareerInsightArticle
            {
                Id = id,
                CategoryId = categoryId,
                CategoryName = categoryName,
                Title = title,
                Summary = summary,
                ImageUrl = imageUrl,
                Author = new CareerInsightAuthor
                {
                    Name = author,
                    AvatarText = string.Join("", author.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(part => part[0])).ToUpperInvariant()
                },
                PublishedOn = publishedOn,
                ReadMinutes = readMinutes,
                IsTrending = isTrending,
                IsNew = isNew,
                IsFeatured = isFeatured,
                Tags = tags
            };
        }
    }
}
