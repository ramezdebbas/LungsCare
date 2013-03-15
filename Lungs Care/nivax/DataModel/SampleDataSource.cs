using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Types",
                    "Types",
                    "Assets/Images/10.jpg",
                    "Lung cancer is cancer that starts in the lungs.The lungs are located in the chest. When you breathe, air goes through your nose, down your windpipe (trachea), and into the lungs, where it spreads through tubes called bronchi. Most lung cancer begins in the cells that line these tubes.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Non-Small Cell",
                    "Non-small cell lung cancer (NSCLC) is the most common type of lung cancer. It usually grows and spreads more slowly than small cell lung cancer.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nThere are three common forms of NSCLC:\n\nAdenocarcinomas are often found in an outer area of the lung.\nSquamous cell carcinomas are usually found in the center of the lung next to an air tube (bronchus).\nLarge cell carcinomas can occur in any part of the lung. They tend to grow and spread faster than the other two types.\n\nSmoking causes most cases of lung cancer. The risk depends on the number of cigarettes you smoke every day and for how long you have smoked. Being around the smoke from other people (secondhand smoke) also raises your risk for lung cancer. However, some people who do not smoke and have never smoked develop lung cancer.\n\nResearch shows that smoking marijuana may help cancer cells grow, but there is no direct link between the drug and developing lung cancer.\n\nHigh levels of air pollution and drinking water containing high levels of arsenic can increase your risk for lung cancer. A history of radiation therapy to the lungs can also increase the risk.\n\nWorking with or near the following cancer-causing chemicals or materials can also increase your risk:\nAsbestos\nChemicals such as uranium, beryllium, vinyl chloride, nickel chromates, coal products, mustard gas, chloromethyl ethers, gasoline, and diesel exhaust\nCertain alloys, paints, pigments, and preservatives Products using chloride and formaldehyde",
                    group1) { CreatedOn = "Group", CreatedTxt = "Types", CreatedOnTwo = "Item", CreatedTxtTwo = "Non-Small Cell", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Lungs Care" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Small Cell",
                     "Small cell lung cancer (SCLC) is a fast-growing type of lung cancer. It spreads much more quickly than non-small cell lung cancer.There are two different types of SCLC:Small cell carcinoma (oat cell cancer)Combined small cell carcinoma",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nAbout 15% of all lung cancer cases are SCLC. Small cell lung cancer is slightly more common in men than women.Almost all cases of SCLC are due to cigarette smoking. SCLC is rare in people who have never smoked.\n\nSCLC is the most aggressive form of lung cancer. It usually starts in the breathing tubes (bronchi) in the center of the chest. Although the cancer cells are small, they grow very quickly and create large tumors. These tumors often spread rapidly (metastasize) to other parts of the body, including the brain, liver, and bone.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Types", CreatedOnTwo = "Item", CreatedTxtTwo = "Small Cell", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Lungs Care" });
            this.AllGroups.Add(group1);

            var group2 = new SampleDataGroup("Group-2",
                   "Symptoms",
                   "Symptoms",
                   "Assets/Images/20.jpg",
                   "Early lung cancer may not cause any symptoms.Symptoms depend on the type of cancer you have, but may include many symptoms");
            group2.Items.Add(new SampleDataItem("Group-2-Item-1",
                    "Chest Pain",
                    "Chest pain is discomfort or pain that you feel anywhere along the front of your body between your neck and upper abdomen.",
                    "Assets/DarkGray.png",
                    "",
                    "Details:\n\nHeart or blood vessel problems that can cause chest pain:\n\nAngina or a heart attack. The most common symptom is chest pain that may feel like tightness, heavy pressure, squeezing, or crushing pain. The pain may spread to the arm, shoulder, jaw, or back.\n\nA tear in the wall of the aorta, the large blood vessel that takes blood from the heart to the rest of the body (aortic dissection) causes sudden, severe pain in the chest and upper back.\nSwelling (inflammation) in the sac that surrounds the heart (pericarditis) causes pain in the center part of the chest.\nLung problems that can cause chest pain:\nA blood clot in the lung (pulmonary embolism)\nCollapse of the lung (pneumothorax)\nPneumonia causes a sharp chest pain that often gets worse when you take a deep breath or cough.\nSwelling of the lining around the lung (pleurisy) can cause chest pain that usually feels sharp, and often gets worse when you take a deep breath or cough.\nOther causes of chest pain:\nPanic attack, which often occurs with fast breathing\nInflammation where the ribs join the breast bone or sternum (costochondritis)\nShingles, which causes sharp, tingling pain on one side that stretches from the chest to the back, and may cause a rash Strain of the muscles and tendons between the ribs Chest pain can also be due to the following digestive system problems:Spasms or narrowing of the esophagus (the tube that carries food from the mouth to the stomach)\nGallstones cause pain that gets worse after a meal (most often a fatty meal)Heartburn or gastroesophageal reflux (GERD)Stomach ulcer or gastritis (burning pain occurs if your stomach is empty and feels better when you eat food)In children, most chest pain is not caused by the heart.",
                    group2) { CreatedOn = "Group", CreatedTxt = "Symptoms", CreatedOnTwo = "Item", CreatedTxtTwo = "Chest Pain", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/21.jpg")), CurrentStatus = "Lungs Care" });
            group2.Items.Add(new SampleDataItem("Group-2-Item-2",
                     "Coughing Up Blood",
                     "Coughing up blood is the spitting up of blood or bloody mucus from the lungs and throat (respiratory tract).Hemoptysis is the medical term for coughing up blood from the respiratory tract.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nA number of conditions, diseases, and medical tests may make you cough up blood, including:\nBlood clot in the lung\nBreathing blood into the lungs (pulmonary aspiration)\nBronchoscopy with biopsy\nBronchiectasis\nBronchitis\nCancer\nCystic fibrosis\nInflammation of the blood vessels in the lung (vasculitis)\nInjury to the arteries of the lungs\nIrritation of the throat from violent coughing (small amounts of blood)\nPneumonia or other lung infections\nPulmonary edema\nSystemic lupus erythematosus\nTuberculosis",
                     group2) { CreatedOn = "Group", CreatedTxt = "Symptoms", CreatedOnTwo = "Item", CreatedTxtTwo = "Coughing Up Blood", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/22.jpg")), CurrentStatus = "Lungs Care" });
            group2.Items.Add(new SampleDataItem("Group-2-Item-3",
                      "Weight Loss",
                      "Unintentional weight loss is a decrease in body weight that is not voluntary. In other words, you did not try to loss the weight by dieting or exercising.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\nThere are many causes of unintentional weight loss. Some are listed below:\nAIDS\nCancer\nDepression\nDiarrhea that is chronic (lasts a long time)\nDrugs, including amphetamines, chemotherapy drugs, laxatives (when abused), and thyroid medications\nDrug abuse\nEating disorders, including anorexia nervosa and bulimia\nHyperthyroidism\nInfection\nLoss of appetite\nMalnutrition\nManipulative behavior (in children)\nPainful mouth sores, mouth braces, or a loss of teeth that prevent you from eating normally\nSmoking",
                      group2) { CreatedOn = "Group", CreatedTxt = "Symptoms", CreatedOnTwo = "Item", CreatedTxtTwo = "Weight Loss", bgColour = "#20B2AA", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/23.jpg")), CurrentStatus = "Lungs Care" });
			group2.Items.Add(new SampleDataItem("Group-2-Item-4",
                      "Appetite",
                      "A decreased appetite is when you have a reduced desire to eat. The medical term for a loss of appetite is anorexia.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\nA decreased appetite is almost always seen among elderly adults, and no cause may be found. However, sadness, depression, grief, or anxiety are a common cause of weight loss that is not explained by other factors, especially among the elderly.\n\nCancer may also cause decreased appetite. You may lose weight without trying. Cancers that may cause you to lose your appetite include:\nColon cancer\nOvarian cancer\nStomach cancer\nPancreatic cancer\nOther causes of decreased appetite include:\nChronic liver disease\nChronic kidney failure\nCOPD\nDementia\nHeart failure\nHepatitis\nHIV\nHypothyroidism\nPregnancy (first trimester)\nUse of certain medications, including antibiotics, chemotherapy drugs, codeine, and morphine\nUse of street drugs including amphetamines (speed), cocaine, and heroin",
                      group2) { CreatedOn = "Group", CreatedTxt = "Symptoms", CreatedOnTwo = "Item", CreatedTxtTwo = "Appetite", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/24.jpg")), CurrentStatus = "Lungs Care" });	
				group2.Items.Add(new SampleDataItem("Group-2-Item-5",
                      "Breathing Difficulty",
                      "Breathing difficulty involves a sensation of difficult or uncomfortable breathing or a feeling of not getting enough air.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\nNo standard definition exists for difficulty breathing. Some people may feel breathless with only mild exercise (for example, climbing stairs), even though they do not have a medical condition. Others may have advanced lung disease but never feel short of breath.\n\nWheezing is one form of breathing difficulty in which you make a high-pitched sound when you breathe out.\nApnea\nBreathing difficulties - first aid\nBreathing difficulties - lying down\nLung diseases\nRapid breathing",
                      group2) { CreatedOn = "Group", CreatedTxt = "Symptoms", CreatedOnTwo = "Item", CreatedTxtTwo = "Breathing Difficulty", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/25.jpg")), CurrentStatus = "Lungs Care" });
				group2.Items.Add(new SampleDataItem("Group-2-Item-6",
                      "Wheezing",
                      "Wheezing is a high-pitched whistling sound during breathing. It occurs when air flows through narrowed breathing tubes.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\nWheezing is a sign that a person may be having breathing problems. The sound of wheezing is most obvious when breathing out (exhaling), but may be heard when taking a breath (inhaling).Wheezing most often comes from the small breathing tubes (bronchial tubes) deep in the chest, but it may be due to a blockage in larger airways or in personswith certain vocal cord problems.",
                      group2) { CreatedOn = "Group", CreatedTxt = "Symptoms", CreatedOnTwo = "Item", CreatedTxtTwo = "Wheezing", bgColour = "#20B2AA", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/26.jpg")), CurrentStatus = "Lungs Care" });
				group2.Items.Add(new SampleDataItem("Group-2-Item-7",
                      "Eyelid Drooping",
                      "Eyelid drooping is excessive sagging of the upper eyelid.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\nA drooping eyelid can stay constant, worsen over time (progressive), or come and go (intermittent). It can be one-sided or on both sides. When drooping is one-sided (unilateral), it is easy to detect by comparing the two eyelids. Drooping is more difficult to detect when it occurs on both sides, or if there is only a slight problem.A furrowed forehead or a chin-up head position may indicate that someone is trying to see under their drooping lids. Eyelid drooping can make someone appear sleepy or tired.Drooping lids are either present at birth (congenital) or develop later in life. A drooping eyelid is not a reason to panic, but you should report it to your doctor.",
                      group2) { CreatedOn = "Group", CreatedTxt = "Symptoms", CreatedOnTwo = "Item", CreatedTxtTwo = "Eyelid Drooping", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/27.jpg")), CurrentStatus = "Lungs Care" });
            this.AllGroups.Add(group2);
			
            var group3 = new SampleDataGroup("Group-3",
                   "Exams & Tests",
                   "Exams & Tests",
                   "Assets/Images/30.jpg",
                   "Many times, lung cancer is found when an x-ray or CT scan is done for another reason.");
            group3.Items.Add(new SampleDataItem("Group-3-Item-1",
                    "Non-Small Cell",
                    "Non-small cell lung cancer (NSCLC) is the most common type of lung cancer. It usually grows and spreads more slowly than small cell lung cancer.",
                    "Assets/DarkGray.png",
                    "",
                    "Details:\n\nThe health care provider will perform a physical exam and ask questions about your medical history. You will be asked if you smoke, and if so, for how long you have smoked.\n\nWhen listening to the chest with a stethoscope, the health care provider can sometimes hear fluid around the lungs, which could (but doesn't always) suggest cancer.\n\nTests that may be performed to diagnose lung cancer or see if it has spread include:\nBone scan\nChest x-ray\nComplete blood count (CBC)\nCT scan of the chest\nMRI of the chest\nPositron emission tomography (PET) scan\nSputum test to look for cancer cells\nThoracentesis (sampling of fluid build-up around the lung)\nIn some cases, the health care provider may need to remove a piece of tissue from your lungs for examination under a microscope. This is called a biopsy.\nThere are several ways to do this:\nBronchoscopy combined with biopsy\nCT-scan-directed needle biopsy\nEndoscopic esophageal ultrasound (EUS) with biopsy\nMediastinoscopy with biopsy\nOpen lung biopsy\nPleural biopsy\nIf the biopsy shows that you do have lung cancer, more imaging tests will be done to determine the stage of the cancer. Stage means how big the tumor is and how far it has spread. Non-small cell lung cancer is divided into five stages:\nStage 0 - the cancer has not spread beyond the inner lining of the lung\n\nStage I - the cancer is small and hasn't spread to the lymph nodes\nStage II - the cancer has spread to some lymph nodes near the original tumor\nStage III - the cancer has spread to nearby tissue or to far away lymph nodes\nStage IV - the cancer has spread to other organs of the body, such as the other lung, brain, or liver",
                    group3) { CreatedOn = "Group", CreatedTxt = "Exams & Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "Non-Small Cell", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/31.jpg")), CurrentStatus = "Lungs Care" });
            group3.Items.Add(new SampleDataItem("Group-3-Item-2",
                     "Small Cell",
                     "Small cell lung cancer (SCLC) is a fast-growing type of lung cancer. It spreads much more quickly than non-small cell lung cancer.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nYour health care provider will perform a physical exam and ask questions about your medical history. You will be asked whether you smoke, and if so, how much and for how long you have smoked.\n\nWhen listening to your chest with a stethoscope, your health care provider can sometimes hear fluid around the lungs or areas where the lung has partially collapsed. Each of these findings could (but does not always) suggest cancer.\n\nSCLC has usually spread to other parts of your body by the time it is diagnosed.\n\nTests that may be performed include:\nBone scan\nChest x-ray\nComplete blood count (CBC)\nCT scan\nLiver function tests\nMRI\nPositron emission tomography (PET) scan\nSputum test (cytology, looking for cancer cells)\nThoracentesis (removal of fluid from the chest cavity around the lungs)\n\nIn most cases, your health care provider may need to remove a piece of tissue from your lungs or other areas to be examined under a microscope. This is called a biopsy. There are several ways to do a biopsy:\nBronchoscopy combined with biopsy\nCT scan-directed needle biopsy\nEndoscopic esophageal ultrasound (EUS) with biopsy\nMediastinoscopy with biopsy\nOpen lung biopsy\nPleural biopsy\nVideo-assisted thoracoscopy\nUsually if a biopsy shows cancer, more imaging tests are done to find out the stage of the cancer. (Stage means how big the tumor is and how far it has spread.) SCLC is classified as either:\nLimited (cancer is only in the chest and can be treated with radiation therapy)\nExtensive (cancer has spread outside the chest)",
                     group3) { CreatedOn = "Group", CreatedTxt = "Exams & Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "Small Cell", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/32.jpg")), CurrentStatus = "Lungs Care" });
            this.AllGroups.Add(group3);
			
			
            var group4 = new SampleDataGroup("Group-4",
                   "Treatment",
                   "Treatment",
                   "Assets/Images/30.jpg",
                   "Treatment options for Lungs Cancer, and acute coronary syndrome, include:Oxygen therapy,Relieving pain and discomfort using nitroglycerin or morphine, Controlling any arrhythmias (abnormal heart rhythms) Blocking further clotting (if possible), using aspirin or clopidogrel (Plavix), as well as possibly anticoagulant drugs such as heparin");
            group4.Items.Add(new SampleDataItem("Group-4-Item-1",
                    "Non-Small Cell",
                    "Non-small cell lung cancer (NSCLC) is the most common type of lung cancer. It usually grows and spreads more slowly than small cell lung cancer.",
                    "Assets/DarkGray.png",
                    "",
                    "Details:\n\nThere are many different types of treatment for non-small cell lung cancer. Treatment depends on the stage of the cancer.Surgery is the often the treatment for patients with non-small cell lung cancer that has not spread beyond nearby lymph nodes. The surgeon may remove:\nOne of the lobes of the lung (lobectomy)Only a small part of the lung (wedge or segment removal)The entire lung (pneumonectomy)Some patients need chemotherapy. Chemotherapy uses drugs to kill cancer cells and stop new cells from growing.Chemotherapy alone is often used when the cancer has spread outside the lung (stage IV).\nIt may also be given before surgery or radiation to make those treatments more effective. This is called neoadjuvant therapy.It may be given after surgery to kill any remaining cancer. This is called adjuvant therapy.\nControlling symptoms and preventing complications during and after chemotherapy is an important part of care.Radiation therapy can be used with chemotherapy if surgery is not possible. Radiation therapy uses powerful x-rays or other forms of radiation to kill cancer cells. Radiation may be used to:\nTreat the cancer, along with chemotherapy, if surgery is not possibleHelp relieve symptoms caused by the cancer, such as breathing problems and swelling\n\nHelp relieve cancer pain when the cancer has spread to the bones\n\nControlling symptoms during and after radiation to the chest is an important part of care.The following treatments are mostly used to relieve symptoms caused by NSCLC:\nLaser therapy - a small beam of light burns and kills cancer cells\nPhotodynamic therapy - uses a light to activate a drug in the body, which kills cancer cells",
                    group4) { CreatedOn = "Group", CreatedTxt = "Treatment", CreatedOnTwo = "Item", CreatedTxtTwo = "Non-Small Cell", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/41.jpg")), CurrentStatus = "Lungs Care" });
            group4.Items.Add(new SampleDataItem("Group-4-Item-2",
                     "Small Cell",
                     "Small cell lung cancer (SCLC) is a fast-growing type of lung cancer. It spreads much more quickly than non-small cell lung cancer.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nBecause SCLC spreads quickly throughout the body, treatment must include cancer-killing drugs (chemotherapy) taken by mouth or injected into the body. Usually, the chemotherapy drug etoposide (or sometimes irinotecan) is combined with either cisplatin or carboplatin.\nCombination chemotherapy and radiation treatment is given to people with SCLC that has spread throughout the body. However, the treatment only helps relieve symptoms. It does not cure the disease.\nRadiation therapy uses powerful x-rays or other forms of radiation to kill cancer cells. Radiation therapy can be used with chemotherapy if surgery is not possible. Radiation may be used to:\nTreat the cancer, along with chemotherapy, if surgery is not possible\nHelp relieve symptoms caused by the cancer, such as breathing problems and swelling\nHelp relieve cancer pain when the cancer has spread to the bonesOften, SCLC may have already spread to the brain, even when there are no symptoms or other signs of cancer in the brain.\n As a result, some patients with smaller cancers, or who had a good response in their first round of chemotherapy may receive radiation therapy to the brain. This method is called prophylactic cranial irradiation (PCI).Surgery helps very few patients with SCLC because the disease has often spread by the time it is diagnosed. Surgery may be done when there is only one tumor that has not spread. If surgery is done, chemotherapy or radiation therapy will still be needed.",
                     group4) { CreatedOn = "Group", CreatedTxt = "Treatment", CreatedOnTwo = "Item", CreatedTxtTwo = "Small Cell", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/42.jpg")), CurrentStatus = "Lungs Care" });
            this.AllGroups.Add(group4);
        }
    }
}
