using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SpeechRecognition
{



    public partial class Form1 : Form
    {
        public string[] keywords = new[] {"Боевик", "Комедия", "Мелодрамма"};
        public string link = "https://www.kinopoisk.ru/s/";
        Label speechText { get; set; }

    public Form1()
        {
            InitializeComponent();
        }

        void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.Write(e.Result.Text);
            speechText.Text = e.Result.Text;
        }


        private void Form1_Shown(object sender, EventArgs e)
        {
                // Настройка Microsoft Speech Recognition
                SpeechRecognitionEngine sre = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("ru-Ru"));
                sre.SetInputToDefaultAudioDevice();
                Grammar testGrammar = new Grammar(new GrammarBuilder("testing"));
                sre.LoadGrammarAsync(testGrammar);
                sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Sre_SpeechRecognized);
                sre.RecognizeAsync();
                speechText = speechRecognitionText;

                if (!string.IsNullOrEmpty(speechText.Text) && speechText.Text != "&&&" && speechText.Text != "???")
                {
                    IWebDriver browser = new ChromeDriver();

                    browser.Navigate().GoToUrl(link);
                    IWebElement searchLinkElement = browser.FindElement(By.Id("find_film"));
                    if (!string.IsNullOrEmpty(speechText.Text) &&
                        !string.IsNullOrEmpty(searchLinkElement.Text))
                    {

                        searchLinkElement.SendKeys(speechText.Text);
                        IWebElement searchClickElement = browser.FindElement(By.ClassName("el_18 submit nice_button"));
                        searchClickElement.Click();
                        browser.Navigate().GoToUrl(string.Concat("ss", browser.Url));
                    }
                }
        }
    }
}