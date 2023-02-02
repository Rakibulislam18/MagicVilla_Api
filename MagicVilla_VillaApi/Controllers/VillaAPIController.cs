using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaApi.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]  
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]

        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);

        }

        [HttpGet("{id:int}",Name="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //[ProducesResponseType(200, Type=typeof(VillaDTO))]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();

            }
            return Ok(villa);

        }
                      ///////////////httpPost//////////create a villa//
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created  )]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {
            //if(!ModelState.IsValid)
            //{ 
            //  return BadRequest(ModelState);
            //}
                      //////match two villa///
            if(VillaStore.villaList.FirstOrDefault(u =>u.Name.ToLower() ==villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "villa already exists!");
                return BadRequest(ModelState);
            }
            if (villaDTO == null)
            {
                return BadRequest();
            }
            if(villaDTO.Id > 0) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDTO.Id =VillaStore.villaList.OrderByDescending(u =>u.Id).FirstOrDefault().Id+1;
            VillaStore.villaList.Add(villaDTO);
             
            return CreatedAtRoute("GetVilla", new {id=villaDTO.Id } , villaDTO);
        }

        //http DElete//

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
       

        [HttpDelete("{id:int}",Name ="DeleteVilla")]
        public IActionResult DeleteVilla(int id)
        {
            if(id==0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if(villa==null)
            {
                return NotFound();
            }
            VillaStore.villaList.Remove(villa);
            return NoContent();



        }

      ///HttPUt-----UPDATE

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "updateVilla")]
        public IActionResult UpdateVilla(int id,[FromBody] VillaDTO villaDTO)
        {
            if (villaDTO ==null || id !=villaDTO.Id)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);

            villa.Name =villaDTO.Name;
            villa.Sqft=villaDTO.Sqft;
            villa.Occupancy =villaDTO.Occupancy;

            return NoContent();
         }
 
        

    }
}
