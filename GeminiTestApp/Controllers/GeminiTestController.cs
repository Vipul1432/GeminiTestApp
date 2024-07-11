using DotnetGeminiSDK.Client;
using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Model;
using DotnetGeminiSDK.Model.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeminiTestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiTestController : ControllerBase
    {
        private IGeminiClient _client;
        public GeminiTestController(IGeminiClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Processes a text prompt and returns the response.
        /// </summary>
        /// <param name="text">The text to prompt.</param>
        /// <returns>A response from the text prompt.</returns>
        [HttpPost("text-prompt")]
        public async Task<ActionResult<string>> TextPromptAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest("Input text cannot be empty.");
            }

            try
            {
                var response = await _client.TextPrompt(text);
                if (response == null)
                {
                    return BadRequest("Text prompt failed.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Processes an image prompt by reading an image file and returning the response.
        /// </summary>
        /// <returns>A response from the image prompt.</returns>
        [HttpPost("image-prompt")]
        public async Task<ActionResult<string>> ImagePromptAsync()
        {
            byte[] image;
            try
            {
                image = await System.IO.File.ReadAllBytesAsync("CURRENTLY_IMAGE_BASE_URL");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to read image file: {ex.Message}");
            }

            try
            {
                var response = await _client.ImagePrompt("Describe this image", image, ImageMimeType.Jpeg);
                if (response == null)
                {
                    return BadRequest("Image prompt failed.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Processes a list of content messages for a text prompt and returns the response.
        /// </summary>
        /// <param name="messages">A list of content messages to prompt.</param>
        /// <returns>A response from the text prompt.</returns>
        [HttpPost("text-prompt-from-messages")]
        public async Task<ActionResult<string>> TextPromptFromMessagesAsync(List<Content> messages)
        {
            if (messages == null || messages.Count == 0)
            {
                return BadRequest("Messages list cannot be null or empty.");
            }

            try
            {
                var response = await _client.TextPrompt(messages);
                if (response == null)
                {
                    return BadRequest("Text prompt failed.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
