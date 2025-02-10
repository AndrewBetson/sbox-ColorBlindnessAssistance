s&box implementation of Electronic Arts' [Tunable Colorblindness Solution](https://github.com/electronicarts/Tunable-Colorblindness-Solution) as a post-processing effect.

Usage
==================
Add the `Post Processing/Color Blindness Assistance` component to each camera your game uses, and expose (some of) the below console elements in your game's options.

Console Elements
==================
This library exposes the following console elements:
| Name | Description | Default | Notes |
|------|------|------|------|
| `r_colorblind_enable` | Enables color blindness assistance. | 0 | None. |
| `r_colorblind_type` | Type of color blindness to compensate for. | 0 | 0: Protanopia, 1: Deuteranopia, 2: Tritanopia |
| `r_colorblind_daltonize_factor` | Strength of the daltonization effect. | 0.0 | Recommended range is 0.0 to 0.9. |
| `r_colorblind_brightness_factor` | Amount to bias the final image's brightness by. | 0.05 | Recommended values are -0.25, -0.12, 0.20, and 0.40. |
| `r_colorblind_contrast_factor` | Amount to bias the final image's contrast by. | 0.20 | Recommended values are -0.25, -0.12, 0.20, and 0.40. |

License
==================
This library is released under the MIT license. For more info, see `LICENSE.md`.

Electronic Arts' Tunable Colorblindness Solution is released under version 2.0 of the Apache license. For more info, see http://www.apache.org/licenses/LICENSE-2.0

TODO
==================
- Apply effect to all cameras globally (maybe with a `GameObjectSystem`?)
