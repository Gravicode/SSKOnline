using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSK.Online.Data
{
    public class AppConstants
    {
        public const float FactorUrea = 2.22f;
        public const float FactorSP36 = 2.77f;
        public const float FactorKCL = 1.66f;
        public const string GemLic = "EDWG-SKFA-D7J1-LDQ5";
        public static Dictionary<string, string> ElementUnits = new Dictionary<string, string> {
{"Bray1_P2O5",   "P₂O₅ - Bray I (ppm)"         },
{"Ca",       "Ca (cmolc/kg)"                   },
{"CLAY",     "Liat (%)"                        },
{"C_N",      "-"                               },
{"HCl25_K2O",    "K₂O - HCl 25% (mg/100g)"     },
{"HCl25_P2O5",   "P₂O₅ - HCl 25% (mg/100g)"    },
{"Jumlah",       "-"                           },
{"K",        "K (cmolc/kg)"                    },
{"KB_adjusted",  "KB (%)"                      },
{"KJELDAHL_N",   "N - total (%)"               },
{"KTK",      "KTK (cmolc/kg)"                  },
{"Mg",       "Mg (cmolc/kg)"                   },
{"Morgan_K2O",   "-"                           },
{"Na",       "Na (cmolc/kg)"                   },
{"Olsen_P2O5",   "P₂O₅ - Olsen (ppm)"          },
{"PH_H2O",       "pH - H₂O"                    },
{"PH_KCL",       "pH KCl"                      },
{"RetensiP", "-"                               },
{"SAND",     "Pasir (%)"                       },
{"SILT",     "Debu (%)"                        },
{"WBC",      "C - org (%)" }
        };
    }
}
