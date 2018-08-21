using System.Collections.Generic;
using BusinessLogic.TestimonialServiceReference;
using System.ServiceModel;
using System.Configuration;

namespace BusinessLogic.Models
{
    public class Testimonial
    {
        #region Testimonial Model operations
        public string EditTestimonialDetails(string testimonialCriteria)
        {
            var testinomialService = this.GetServiceClient();
            var testinomialDetails = testinomialService.GetTestimonialSpecificDetails(testimonialCriteria);
            return testinomialDetails;
        }
        #endregion

        #region Delete Current Testimonial
        public string DeleteCurrentTestimonial(string testimonialCriteria)
        {
            var testinomialService = this.GetServiceClient();
            var result = testinomialService.DeleteCurrentTestimonial(testimonialCriteria);
            return result;
        }
        #endregion

        #region Save Testimonial Details
        public string SaveTestimonialDetails(string testimonialCriteria)
        {
            var testinomialService = this.GetServiceClient();
            var result = testinomialService.SaveTestimonialDetails(testimonialCriteria);
            return result;
        }
        #endregion

        #region Get all Testimonial
        public List<BusinessObjects.TestimonialModel> GetAllTestimonial(string criteria = null)
        {
            var testinomialService = this.GetServiceClient();
            List<BusinessObjects.TestimonialModel> lstTestimonialModel = new List<BusinessObjects.TestimonialModel>();
            var testinomialDetails = testinomialService.GetAllTestimonial(criteria);

            foreach (var testinomial in testinomialDetails)
            {
                BusinessObjects.TestimonialModel testimonialModel = new BusinessObjects.TestimonialModel();
                testimonialModel.ID = testinomial.ID;
                testimonialModel.Description = testinomial.Description;
                testimonialModel.Author = testinomial.Author;
                testimonialModel.ImagePath = testinomial.ImagePath;
                testimonialModel.IsActive = testinomial.IsActive;
                testimonialModel.CreatedDate = testinomial.CreatedDate;
                testimonialModel.ModifiedDate = testinomial.ModifiedDate;
                lstTestimonialModel.Add(testimonialModel);
            }

            List<BusinessObjects.TestimonialModel> TestimonialList = new List<BusinessObjects.TestimonialModel>();
            foreach (var item in lstTestimonialModel)
            {
                TestimonialList.Add(item);
            }
            return TestimonialList;
        }
        #endregion

        #region Update Testimonial Image Path
        public string UpdateTestimonialImagePath(string testimonialCriteria)
        {
            var testinomialService = this.GetServiceClient();
            var result = testinomialService.UpdateTestimonialImagePath(testimonialCriteria);
            return result;
        }
        #endregion

        #region Get Testimonial service reference
        private ITestimonialService GetServiceClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            var testimonialServiceReference = ConfigurationManager.AppSettings["TestimonialService"];
            EndpointAddress endpoint = new EndpointAddress(testimonialServiceReference);
            ITestimonialService client = ChannelFactory<ITestimonialService>.CreateChannel(binding, endpoint);
            return client;
        }
        #endregion
    }
}
