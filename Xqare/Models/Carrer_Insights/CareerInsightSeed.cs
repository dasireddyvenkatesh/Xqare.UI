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
            Article("genai-2026", "industry", "Industry Insights", "Generative AI Skills Every IT Professional Must Learn in 2026", "The 5 GenAI skills that actually get you hired in 2026: prompting, RAG, evaluation, agents, and production integration. Skip the hype, build what pays.", "https://tseqcbvvjdwmkzifpbnb.supabase.co/storage/v1/object/public/Career%20Insights/Generative%20AI%20Skills%20Every%20IT%20Professional%20Must%20Learn%20in%202026.jpg", "Ishaan Verma", new DateTime(2026, 3, 10), 12, true, true, true, ["ai", "rag", "agents", "career"]),
            Article("faang-offer", "job-preparation", "Job Preparation", "Landing Your First FAANG Offer: The Insider Playbook", "A step-by-step playbook from XQARE mentors. Learn the rules and prep rhythm.", "https://tseqcbvvjdwmkzifpbnb.supabase.co/storage/v1/object/public/Career%20Insights/Landing%20Your%20First%20FAANG%20Offer%20The%20Insider%20Playbook.jpg", "Aditi Ramanathan", new DateTime(2026, 3, 9), 13, false, false, false, ["interviews", "faang", "offers"]),
            Article("linkedin-guide", "career-guidance", "Career Guidance", "The Ultimate LinkedIn Profile Optimization Guide for Tech Professionals", "A 60-minute LinkedIn optimization sprint that turns your profile into a passive job engine, with the exact sections recruiters scan first.", "https://tseqcbvvjdwmkzifpbnb.supabase.co/storage/v1/object/public/Career%20Insights/The%20Ultimate%20LinkedIn%20Profile%20Optimization%20Guide%20for%20Tech%20Professionals.jpg", "Sanya Kapoor", new DateTime(2026, 3, 8), 10, false, false, false, ["linkedin", "personal brand"]),
            Article("mentorship-growth", "mentorship", "Mentorship & Professional Growth", "How Mentorship Can Accelerate Your Tech Career", "Discover how connecting with the right mentor can fast-track your growth, open doors to new opportunities, and sharpen your decisions.", "https://tseqcbvvjdwmkzifpbnb.supabase.co/storage/v1/object/public/Career%20Insights/How%20Mentorship%20Can%20Accelerate%20Your%20Tech%20Career.jpg", "Priya Sharma", new DateTime(2026, 3, 5), 7, false, false, false, ["mentorship", "growth"]),
            Article("cloud-roadmap", "career-guidance", "Career Guidance", "Roadmap to Become a Cloud Engineer", "A step-by-step guide covering certifications, skills, and projects you need to land your first cloud engineering role.", "https://tseqcbvvjdwmkzifpbnb.supabase.co/storage/v1/object/public/Career%20Insights/Roadmap%20to%20Become%20a%20Cloud%20Engineer.jpg", "Arjun Mehta", new DateTime(2026, 3, 2), 6, false, false, false, ["cloud", "roadmap"]),
            Article("technical-interviews", "job-preparation", "Job Preparation", "How to Prepare for Technical Interviews", "Proven strategies and resources to help you ace coding interviews and system design rounds at top tech companies.", "https://tseqcbvvjdwmkzifpbnb.supabase.co/storage/v1/object/public/Career%20Insights/How%20to%20Prepare%20for%20Technical%20Interviews.jpg", "Neha Gupta", new DateTime(2026, 2, 28), 8, false, false, false, ["coding", "system design"]),
            Article("hiring-skills", "industry", "Industry Insights", "Top Skills Tech Companies Are Hiring For", "Stay ahead of the curve by learning which technical and soft skills are most in demand across the industry right now.", "https://tseqcbvvjdwmkzifpbnb.supabase.co/storage/v1/object/public/Career%20Insights/Top%20Skills%20Tech%20Companies%20Are%20Hiring%20For.jpg", "Ravi Patel", new DateTime(2026, 2, 25), 5, false, false, false, ["skills", "hiring"]),
            Article("resume-tips", "job-preparation", "Job Preparation", "Resume Tips for IT Professionals", "Learn how to craft a compelling resume that stands out to recruiters and passes through applicant tracking systems.", "https://tseqcbvvjdwmkzifpbnb.supabase.co/storage/v1/object/public/Career%20Insights/Resume%20Tips%20for%20IT%20Professionals.jpg", "Meera Iyer", new DateTime(2026, 2, 20), 9, false, false, false, ["resume", "ats"]),
            Article("data-engineering", "career-guidance", "Career Guidance", "How to Transition into Data Engineering", "A practical guide for developers and analysts looking to pivot into one of the fastest-growing roles in tech.", "https://tseqcbvvjdwmkzifpbnb.supabase.co/storage/v1/object/public/Career%20Insights/How%20to%20Transition%20into%20Data%20Engineering.jpg", "Karan Shah", new DateTime(2026, 2, 18), 11, false, false, false, ["data", "engineering"]),
            Article("fresher-tech-lead", "career-guidance", "Career Guidance", "From Fresher to Tech Lead: A Career Growth Blueprint", "Map out the key milestones, skills, and mindset shifts required to grow from your first tech role into technical leadership.", "https://tseqcbvvjdwmkzifpbnb.supabase.co/storage/v1/object/public/Career%20Insights/From%20Fresher%20to%20Tech%20Lead%20A%20Career%20Growth%20Blueprint.jpg", "Anika Rao", new DateTime(2026, 2, 15), 8, false, false, false, ["leadership", "growth"])
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
