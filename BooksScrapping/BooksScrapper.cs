using BookStoreModels;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BooksScrapping
{
    public class BooksScrapper
    {
        private readonly IWebDriver driver;
        public BooksScrapper()
        {
            driver = new ChromeDriver();
            //_baseUrl = baseUrl;
        }

        public BooksAndAuthors GetDataFromUrl(string url)
        {
            driver.Navigate().GoToUrl(url);

            var books = new List<Book>();
            var authors = new List<Author>();


            var count = Convert.ToInt32(driver
                .FindElement(By
                .XPath
                ("//*[@id=\"allcontent\"]/section/section/section/section[2]/div[1]/div[2]/div")).Text);

            for (var i = 1; i <= count; i++)
            {
                var linkElement = driver
                    .FindElement(By.XPath($"//*[@id=\"allcontent\"]/section/section/section/section[3]/section[{i}]/div[2]/div[2]/a"));
                linkElement.Click();

                var infoBlock = driver.FindElement(By.ClassName("prd-m-info-block"));
                var author = new Author
                {
                    //Name = driver.FindElement(By.XPath("//*[@id=\"allcontent\"]/section/section/section/section[1]/article/div[1]")).Text
                    Name = infoBlock.FindElement(By.ClassName("prd-author-name")).Text
                };

                authors.Add(author);

                var book = new Book
                {
                    Name = driver.FindElement(By.XPath("//*[@id=\"allcontent\"]/section/section/section/section[1]/article/header/h1")).Text,
                    ImageUrl = driver.FindElement(By.XPath("//*[@id=\"allcontent\"]/section/section/section/section[1]/div/div[1]/a/img")).GetAttribute("src"),
                    Price = ParseTextPriceToDouble(driver.FindElement(By.ClassName("prd-your-price-numb")).Text),
                    Description = driver.FindElement(By.XPath("//*[@id=\"allcontent\"]/section/section/section/section[3]/article/div[1]")).Text,
                };

                books.Add(book);

                driver.Navigate().Back();
            }
            return new BooksAndAuthors { Authors = authors, Books = books};
        }

        public static double ParseTextPriceToDouble(string textPrice)
        {
            var res = "";
            for (var i = 0; i < textPrice.Length; i++)
            {
                res += textPrice[i];
                if (textPrice[i] == ' ')
                {
                    break;
                }
            }
            return Convert.ToDouble(res);
        }

        public void QuitDriver()
        {
            driver.Quit();
        }

    }
}
